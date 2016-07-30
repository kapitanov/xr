using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace AISTek.XRage
{
    public static class XMath
    {
        public static bool IsPowerOfTwo(int value)
        {
            if (value < 2)
            {
                return false;
            }

            if ((value & (value - 1)) == 0)
            {
                return true;
            }

            return false;
        }

        public static void Swap<T>(ref T x, ref T y)
        {
            var oldX = x;
            x = y;
            y = oldX;
        }

        public static Vector3? GetIntersectionPoint(Plane p1, Plane p2, Plane p3)
        {
            var denominator = (Vector3.Dot(p1.Normal, Vector3.Cross(p2.Normal, p3.Normal)));
            if (denominator == 0.0f)
            {
                // No intersection
                return null;
            }

            return ((-p1.D * Vector3.Cross(p2.Normal, p3.Normal)) +
                    (-p2.D * Vector3.Cross(p3.Normal, p1.Normal)) +
                    (-p3.D * Vector3.Cross(p1.Normal, p2.Normal))) / denominator;
        }

        public static IEnumerable<Vector3> OrderClockwise(this IEnumerable<Vector3> vertices, Vector3[] faceVertices, Vector3 normal)
        {
            var centerPoint = vertices.Aggregate((x, y) => x + y) / vertices.Count();
            
            var vertexA = faceVertices[0];
            var vertexB = faceVertices[1];
            var vertexC = faceVertices[2];

            if (Math.Sign(Vector3.Dot(normal, new Plane(vertexA, vertexB, vertexC).Normal)) < 0)
            {
                vertexA = faceVertices[2];
                vertexC = faceVertices[0];
            }
            
            var u = Vector3.Normalize(vertexB - vertexA);
            var v = Vector3.Normalize(vertexC - u * Vector3.Dot(vertexC, u));

            // Vectors u and v form local coordinate system

            var centerAngle = (float)Math.Atan2(
               Vector3.Dot(centerPoint - vertexA, v),
               Vector3.Dot(centerPoint - vertexA, u));

            return from vertex in
                       (from vertex in vertices
                        let projectedVertex = new Vector2(
                            Vector3.Dot(vertex - vertexA, u),
                            Vector3.Dot(vertex - vertexA, v))
                        let angle = (float)Math.Atan2(projectedVertex.Y, projectedVertex.X)
                        let angleFactor = MathHelper.ToDegrees(centerAngle - angle)
                        select new { Vertex = vertex, ProjectedVertex = projectedVertex, Factor = angleFactor })
                   orderby vertex.Factor
                   select vertex.Vertex;
        }

        public static IEnumerable<Vector3> OrderClockwise(this IEnumerable<Vector3> vertices, Vector3 normalDirection)
        {
            return OrderClockwiseOrCounterClockwise(vertices, normalDirection, true);
        }

        public static IEnumerable<Vector3> OrderCounterClockwise(this IEnumerable<Vector3> vertices, Vector3 normalDirection)
        {
            return OrderClockwiseOrCounterClockwise(vertices, normalDirection, false);
        }

        private static IEnumerable<Vector3> OrderClockwiseOrCounterClockwise(IEnumerable<Vector3> verticesEnumerable, Vector3 normalDirection, bool clockWise)
        {
            var vertices = verticesEnumerable.ToList();
            var centerPoint = vertices.Aggregate((x, y) => x + y) / vertices.Count();

            var vertexA = vertices.First();
            var vertexB = vertices.Skip(1).First();
            var vertexC = vertices.Skip(2).First();

            var normal = new Plane(vertexA, vertexB, vertexC).Normal;
            Debug.Print("XMath normal: {0}, face normal:  {1}, {2}", normal, normalDirection, clockWise ? "CW" : "CCW");
            if (Vector3.Dot(normal, normalDirection) < 0.0f)
            {
                Debug.Print("Inverted");
                XMath.Swap(ref vertexA, ref vertexC);            
            }

            var u = Vector3.Normalize(vertexA - vertexB);
            var v = Vector3.Normalize(
                vertexA - vertexC
                //vertexC - u * Vector3.Dot(vertexC, u)
                );

            Debug.Print("U: {0}    V: {1}", u, v);

            var centerAngle = (float)Math.Atan2(
               Vector3.Dot(centerPoint - vertexA, v),
               Vector3.Dot(centerPoint - vertexA, u));

            var vrt = from vertex in vertices
                      let projectedVertex = new Vector2(
                          Vector3.Dot(vertex - vertexA, u),
                          Vector3.Dot(vertex - vertexA, v))
                      let angle = (float)Math.Atan2(projectedVertex.Y, projectedVertex.X)
                      let angleDegrees = MathHelper.ToDegrees(centerAngle - angle)
                      let angleFactor = clockWise ? angleDegrees : -angleDegrees
                      select new { Vertex = vertex, ProjectedVertex = projectedVertex, Factor = angleFactor };

            return vrt.OrderByDescending(x => x.Factor).Select(x => x.Vertex);


            normal = new Plane(vertexA, vertexB, vertexC).Normal;
            Debug.Print("XMath normal: {0}, face normal:  {1}, {2}", normal, normalDirection, clockWise ? "CW" : "CCW");
            if (Vector3.Dot(normal, normalDirection) < 0.0f)
            {
                Debug.Print("Inverted");
                return vrt.OrderByDescending(x => x.Factor).Select(x => x.Vertex);
            }

            Debug.Print("Original");
            return vrt.OrderBy(x => x.Factor).Select(x => x.Vertex);
        }

        public static Vector2 ProjectOnto(this Vector3 vector, Plane plane)
        {
            var vectorA = new Vector3(-plane.D / plane.Normal.X, 0, 0);
            var vectorB = new Vector3(0, -plane.D / plane.Normal.Y, 0);
            var vectorC = new Vector3(0, 0, -plane.D / plane.Normal.Z);

            return vector.ProjectOnto(vectorA, vectorB, vectorC);
        }

        public static Vector2 ProjectOnto(this Vector3 vector, Vector3 vectorA, Vector3 vectorB, Vector3 vectorC)
        {
            var u = Vector3.Normalize(vectorA - vectorB);
            var v = vectorC - u * Vector3.Dot(vectorC, u);

            return new Vector2(Vector3.Dot(vector, v), Vector3.Dot(vector, u));
        }

        public static float GetGaussianDistribution(float x, float y, float rho)
        {
            var g = 1.0f / (float)Math.Sqrt(2.0f * 3.141592654f * rho * rho);
            return g * (float)Math.Exp(-(x * x + y * y) / (2 * rho * rho));
        }
    }
}
