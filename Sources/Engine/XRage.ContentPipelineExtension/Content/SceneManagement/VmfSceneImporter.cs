using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using AISTek.XRage.Content.VmfParsing;
using System.Diagnostics;

namespace AISTek.XRage.Content.SceneManagement
{
    [ContentImporter(".vmf", DisplayName = "VMF scene importer")]
    public class VmfSceneImporter : ContentImporter<CompiledVmfScene>
    {
        public override CompiledVmfScene Import(string filename, ContentImporterContext context)
        {
            var text = File.ReadAllText(filename);
            var vmfTree = vmfParser.ReadVmf(text);

            var scene = BuildScene(filename, vmfTree);
            Trace.TraceInformation("VMF map {0}: {1} brushes ({2} skipped), {3} entities", scene.Name, scene.Brushes.Count, scene.BrushesSkipped, scene.StaticEntities.Count);
            return scene;
        }

        private CompiledVmfScene BuildScene(string filename, VmfDocument vmf)
        {
            var brushesSkipped = 0;
            var solids = vmf.Children.OfType<VmfClassNode>()
                                     .Where(node => node.Name == "world")
                                     .SelectMany(node => node.Children.Where(n => n.Name == "solid")
                                                                      .OfType<VmfClassNode>())
                                     .ToList();

            var brushes = from solid in solids
                          // let material = solid.OfType<VmfAttributeNode>().First(n => n.Name == "material").Value
                          let material = solid.Children.OfType<VmfClassNode>()
                                              .First(n => n.Name == "side")
                                              .Children.OfType<VmfPropertyNode>()
                                              .First(n => n.Name == "material").Value

                          //let material = "no_material"

                          let planes = from side in solid.Children.OfType<VmfClassNode>()
                                       where side.Name == "side"
                                       let vertices = ConvertVertices(side.ReadVertices("plane"))
                                       select new PlaneDefinition(vertices)

                          let brushSides = CreateBrushSides(planes).ToList()

                          select new CompiledVmfBrush { MaterialPath = material, Sides = brushSides };

            var scene = new CompiledVmfScene(Path.GetFileNameWithoutExtension(filename))
            {
                Brushes = brushes.Where(brush =>
                {
                    if (brush.Sides.Count > 0)
                        return true;

                    brushesSkipped++;
                    return false;
                }).ToList()
            };
            scene.BrushesSkipped = brushesSkipped;

            return scene;
        }

        private static Vector3[] ConvertVertices(Vector3[] vertices)
        {
            return vertices.Select(vertex => new Vector3(vertex.X, vertex.Z, -vertex.Y)).ToArray();
        }

        static Color[] colors = new Color[] 
        { 
            Color.Red, 
            Color.Green, 
            Color.White, 
            Color.Blue, 
            Color.Yellow, 
            Color.Pink
        };

        private static IEnumerable<Color> InfiniteColors()
        {
            while (true)
            {
                foreach (var color in colors)
                    yield return color;
            }
        }

        static List<Vector3> vertices;
        static List<PlaneDefinition> adjacentPlanes;

        private static IEnumerable<VmfBrushSide> CreateBrushSides(IEnumerable<PlaneDefinition> planes)
        {
            // Calc center of the brush
            // We're going to need it for face normals' correction
            var centerOfBrush = planes.SelectMany(plane => plane.Vertices)
                                      .Aggregate((x, y) => x + y)
                                      / planes.SelectMany(plane => plane.Vertices).Count();
            Debug.Print("**********************************************");
            Debug.Print("Brush center: {0}", centerOfBrush);

            foreach (var pc in planes.Zip(InfiniteColors(), (p, c) => new { p, c = c.ToVector3() }))
            {
                var firstPlane = pc.p;
                var polygonCenter = firstPlane.Vertices.Aggregate((x, y) => x + y) / 3f;

                var maxAllowedR = (float)Math.Ceiling((from p1 in firstPlane.Vertices
                                                       from p2 in firstPlane.Vertices
                                                       let d = p1 - p2
                                                       let projection = d.ProjectOnto(firstPlane.Vertices[0], firstPlane.Vertices[1], firstPlane.Vertices[2])
                                                       let r = projection.Length()
                                                       select r).Max());

                Debug.Print("Plane {0}:", firstPlane);
                vertices = new List<Vector3>();
                adjacentPlanes = planes.Where(plane => !plane.IsSame(firstPlane)).ToList();

                var normal = firstPlane.Plane.Normal;
                if(Math.Sign(Vector3.Dot(firstPlane.Plane.Normal, polygonCenter - centerOfBrush)) < 0)
                {
                    //firstPlane.Plane = new Plane(-firstPlane.Plane.Normal, firstPlane.Plane.D);
                    normal *= -1.0f;
                    Debug.Print("Normal inverted: {0}", firstPlane.Plane.Normal);
                }

                Debug.Print("Normal {0}", normal);

                foreach (var pair in from p1 in adjacentPlanes
                                     from p2 in adjacentPlanes
                                     where !p1.IsSame(p2)
                                     select new { SecondPlane = p1, ThirdPlane = p2 })
                {
                    var intersectionPoint = XMath.GetIntersectionPoint(firstPlane.Plane, pair.SecondPlane.Plane, pair.ThirdPlane.Plane);
                    if (intersectionPoint.HasValue)
                    {
                        vertices.Add(intersectionPoint.Value);
                    }
                }
                
                // Remove duplicates and false vertices
                vertices = vertices.GroupBy(x => x)
                                   .Select(x => x.Key)
                                   .Select(x => new
                                   {
                                       Vertex = x,
                                       Radius = (x - polygonCenter).ProjectOnto(firstPlane.Vertices[0], firstPlane.Vertices[1], firstPlane.Vertices[2])
                                   })
                                   .Where(x =>  x.Radius.Length() <= maxAllowedR)
                                   .Select(x => x.Vertex)
                                   .ToList();

                if (vertices.Count > 2)
                {
                    vertices = vertices.OrderClockwise(normal).ToList();

                }

                switch (vertices.Count)
                {
                    case 0:
                        Trace.TraceWarning("VmSceneImporter: found plane {0} that doesn't form any brush sides.", firstPlane);
                        break;

                    case 1:
                        Trace.TraceWarning("VmSceneImporter: found plane {0} that forms only one vertex.", firstPlane);
                        break;

                    case 2:
                        Trace.TraceWarning("VmSceneImporter: found plane {0} that forms only two vertices.", firstPlane);
                        break;

                    case 3:
                        Debug.Print("Created face: ({0} {1} {2})",
                            PlaneDefinition.VectorName(vertices[0]), PlaneDefinition.VectorName(vertices[1]), PlaneDefinition.VectorName(vertices[2]));
                        yield return new VmfBrushSide(vertices[0], vertices[1], vertices[2], normal);
                        break;

                    case 4:
                        Debug.Print("Created face: ({0} {1} {2})",
                            PlaneDefinition.VectorName(vertices[0]), PlaneDefinition.VectorName(vertices[1]), PlaneDefinition.VectorName(vertices[2]));
                        Debug.Print("Created face: ({0} {1} {2})",
                            PlaneDefinition.VectorName(vertices[0]), PlaneDefinition.VectorName(vertices[2]), PlaneDefinition.VectorName(vertices[3]));
                        yield return new VmfBrushSide(vertices[0], vertices[1], vertices[2], normal);
                        yield return new VmfBrushSide(vertices[0], vertices[2], vertices[3], normal);
                        break;

                    default:
                        // TODO: sides with more than 4 vertices are not correctly processed yet.
                        for (var i = 1; i < vertices.Count - 1; i++)
                        {
                            yield return new VmfBrushSide(vertices[0], vertices[i], vertices[i + 1], normal);
                        }


                            //   Trace.TraceWarning("VmSceneImporter: found brush side with {0} vertices. Brush sides with more that 4 vertices are not supported yet.", vertices.Count);
                            break;
                }
            }
        }

        private class PlaneDefinition
        {
            public PlaneDefinition(Vector3[] vertices)
            {
                Vertices = vertices;
                Plane = new Plane(vertices[0], vertices[1], vertices[2]);

                if (Plane.D == float.NaN)
                    throw new ArgumentException();

                if (Plane.Normal.X == float.NaN)
                    throw new ArgumentException();
                if (Plane.Normal.Y == float.NaN)
                    throw new ArgumentException();
                if (Plane.Normal.Z == float.NaN)
                    throw new ArgumentException();
            }

            public Vector3[] Vertices { get; private set; }

            public Plane Plane { get; set; }

            public bool IsAdjacent(PlaneDefinition plane)
            {
                if (IsSame(plane))
                {
                    return false;
                }

                foreach (var x in Vertices)
                {
                    foreach (var y in plane.Vertices)
                    {
                        if (x == y)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            public bool IsSame(PlaneDefinition other)
            {
                if (other == null)
                    return false;

                // Compare all vertices
                var equals = Vertices.Zip(other.Vertices, (x, y) => new { x, y }).All(p => p.x == p.y);
                return equals;
            }

            public override string ToString()
            {
                return string.Format("({1} {2} {3})", Plane, VectorName(Vertices[0]), VectorName(Vertices[1]), VectorName(Vertices[2]));
            }

            public static string VectorName(Vector3 v)
            {
                if (names.ContainsKey(v))
                    return names[v];

                return v.ToString();
            }

            private static Dictionary<Vector3, string> names = new Dictionary<Vector3, string> 
            {
                // Brush 1
                { new Vector3(512,1024,1024),"A1"},
                { new Vector3(1024,1024,1024),"B1"},
                { new Vector3(512,512,1024),"C1"},
                { new Vector3(1024,512,1024),"D1"},

                { new Vector3(512,1024,512),"E1"},
                { new Vector3(1024,1024,512),"F1"},
                { new Vector3(512,512,512),"G1"},
                { new Vector3(1024,512,512),"H1"},

                // Brush 2
                { new Vector3(-1024,-512,-512),"A2"},
                { new Vector3(-512,-512,-512),"B2"},
                { new Vector3(-1024,-1024,-512),"C2"},
                { new Vector3(-512,-1024,-512),"D2"},

                { new Vector3(-1024,-512,-1024),"E2"},
                { new Vector3(-512,-512,-1024),"F2"},
                { new Vector3(-1024,-1024,-1024),"G2"},
                { new Vector3(-512,-1024,-1024),"H2"},
            };
        }

        private VmfParser vmfParser = new VmfParser();
    }
}
