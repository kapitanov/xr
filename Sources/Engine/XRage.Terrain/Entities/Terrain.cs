using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace AISTek.XRage.Entities
{
    public class Terrain  : BaseEntity
    {
        public Terrain(XGame game, string heightMapPath, string materialPath)
            : base(game, "terrain")
        {
            this.heightMapPath = heightMapPath;
            this.materialPath = materialPath;

            Vertices = new List<VertexTerrain>();
            MinimumLeafSize = 256;
            ElevationFactor = 1.0f;
            ScaleFactor = 75.0f;

            Statistics = new TerrainVisualizationStatistics();
        }

        internal VertexBuffer VertexBuffer { get; private set; }

        internal IList<VertexTerrain> Vertices { get; private set; }

        internal QuadTree Root { get; private set; }

        public int Size { get; private set; }

        public float ScaleFactor
        {
            get { return scaleFactor; }
            set
            {
                scaleFactor = value;
            }
        }

        public float ElevationFactor
        {
            get { return elevationFactor; }
            set
            {
                elevationFactor = value;
            }
        }

        public int MinimumLeafSize { get; private set; }

        internal float[,] HeightData { get; private set; }

        internal float MinimumElevation { get;private set; }

        internal float MaximumElevation { get; private set; }

        internal Vector3[,] Normals { get; private set; }

        internal TerrainVisualizationStatistics Statistics { get; private set; }

        public Material Material { get; private set; }

        public override void LoadContent()
        {
            LoadHeightData();
            CalculateNormals();
            Root = new QuadTree(this, Size);
          //  CreateVertexBuffer();
            HeightData = null;
            Vertices.Clear();

            LoadMaterial();
        }

        public override void Update(GameTime gameTime)
        {  }

        public override void QueryForChunks(ref RenderPassDescriptor pass)
        {
            Statistics.ClearStatistics();
            Root.QueryForChunks(ref pass);
        }

        public override void UnloadContent()
        {
            Root.Dispose();
            //VertexBuffer.Dispose();
            Material.Dispose();
        }
        
        private void LoadHeightData()
        {
            var heightMap = Game.Content.Load<Texture2D>(heightMapPath);

            if (!XMath.IsPowerOfTwo(heightMap.Width) ||
                !XMath.IsPowerOfTwo(heightMap.Height))
            {
                throw new Exception("Height maps must have a size and height that is a power of two.");
            }

            Size = heightMap.Width;

            var heightMapColors = new Color[Size * Size];
            heightMap.GetData(heightMapColors);

            HeightData = new float[Size, Size];
            MinimumElevation = float.PositiveInfinity;
            MaximumElevation = float.NegativeInfinity;

            for (int x = 0; x < Size; x++)
            {
                for (int z = 0; z < Size; z++)
                {
                    var color = heightMapColors[z + x * Size];
                    var height = ElevationFactor * (color.R + color.G + color.B);

                    if (height > MaximumElevation)
                        MaximumElevation = height;
                    if (height < MinimumElevation)
                        MinimumElevation = height;

                    HeightData[z, x] = height;
                }
            }

            Debug.Print("Terrain range: [{0} - {1}]", MinimumElevation, MaximumElevation);
        }

        private void LoadMaterial()
        {
            Material = Game.Content.Load<Material>(materialPath);
        }

        private void CalculateNormals()
        {
            var terrainVertices = new VertexTerrain[Size * Size];
            Normals = new Vector3[Size, Size];

            for (int x = 0; x < Size; x++)
            {
                for (int z = 0; z < Size; z++)
                {
                    terrainVertices[x + z * Size].Position = new Vector3(x * ScaleFactor, HeightData[x, z], z * ScaleFactor);
                }
            }

            // Setup normals for lighting and physics (Credit: Riemer's method)
            int sizeMinusOne = Size - 1;
            for (int x = 1; x < sizeMinusOne; x++)
            {
                for (int z = 1; z < sizeMinusOne; z++)
                {
                    int zTimesSize = (z * Size);
                    var normX = new Vector3(
                        (terrainVertices[x - 1 + zTimesSize].Position.Y - terrainVertices[x + 1 + zTimesSize].Position.Y) / 2,
                        1,
                        0);
                    var normZ = new Vector3(
                        0,
                        1,
                        (terrainVertices[x + (z - 1) * Size].Position.Y - terrainVertices[x + (z + 1) * Size].Position.Y) / 2);

                    // We inline the normalize method here since it is used alot, this is faster than calling Vector3.Normalize()
                    var normal = normX + normZ;
                    var length = (float)Math.Sqrt((float)((normal.X * normal.X) + (normal.Y * normal.Y) + (normal.Z * normal.Z)));
                    var num = 1f / length;
                    normal.X *= num;
                    normal.Y *= num;
                    normal.Z *= num;

                    Normals[x, z] = terrainVertices[x + zTimesSize].Normal = normal;
                    // Stored for use in physics and for the
                    // quad-tree component to reference.
                }
            }
        }

        private void CreateVertexBuffer()
        {
            var verticesArray = new VertexTerrain[Vertices.Count];
            Vertices.CopyTo(verticesArray, 0);
            VertexBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexTerrain), Vertices.Count, BufferUsage.WriteOnly);
            VertexBuffer.SetData(verticesArray);
        }

        private float scaleFactor;
        private float elevationFactor;
        private int minLeafSize;
        private string heightMapPath;
        private string materialPath;
    }
}
