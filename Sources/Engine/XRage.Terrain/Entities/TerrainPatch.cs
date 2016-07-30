using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;
using System.Diagnostics;

namespace AISTek.XRage.Entities
{
    internal class TerrainPatch : IDisposable
    {
        public TerrainPatch(QuadTree parentNode)
        {
            ParentNode = parentNode;
            CreateVertices();
            CreateIndices();
        }

        public QuadTree ParentNode { get; private set; }

        public Terrain Terrain { get { return ParentNode.Terrain; } }

        public XGame Game { get { return ParentNode.Terrain.Game; } }

        public VertexBuffer VertexBuffer { get; private set; }

        public IndexBuffer IndexBuffer { get; private set; }

        public int VertexBufferOffset { get; private set; }

        public int NumVertices { get; private set; }

        public Vector3 Position { get; private set; }

        public void CreateVertices()
        {
            var size = ParentNode.Size;
            //VertexBufferOffset = Terrain.Vertices.Count;
            vertices = new VertexTerrain[size * size];

            var halfSize = Terrain.Size / 2;
            var halfNodeSize = size / 2;
            var j = 0;

            for (var x = -halfNodeSize; x < halfNodeSize; x++)
            {
                for (var z = -halfNodeSize; z < halfNodeSize; z++)
                {
                    var tx = ParentNode.OffsetX + x;
                    var tz = ParentNode.OffsetZ + z;

                    var hx = tx + halfSize;
                    var hz = tz + halfSize;

                    var offset = x + z * size;

                    var vertex = new VertexTerrain
                    {
                        Position = new Vector3(
                            tx * Terrain.ScaleFactor,
                            Terrain.HeightData[hx, hz],
                            tz * Terrain.ScaleFactor),
                        Normal = Terrain.Normals[hx, hz],
                        TexCoord = new Vector2(hx, hz) / Terrain.Size
                    };

                    vertices[j++] = vertex;
                    //Terrain.Vertices.Add(vertex);
                }
            }


            var indices = new int[size * size * 6];
            var counter = 0;
            Func<int, int, int> calcIndex = (_x, _z) =>
            {
                //_x += ParentNode.OffsetX / 2 + Terrain.Size / 2;
                //_z += ParentNode.OffsetZ / 2 + Terrain.Size / 2;

                if (_x >= size)
                    _x = size - 1;
                if (_z >= size)
                    _z = size - 1;

                return _x + _z * size;
            };

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var index0 = calcIndex(x, y);
                    var index1 = calcIndex(x + 1, y);
                    var index2 = calcIndex(x, y + 1);
                    var index3 = calcIndex(x + 1, y + 1);

                    indices[counter++] = index0;
                    indices[counter++] = index2;
                    indices[counter++] = index1;

                    indices[counter++] = index1;
                    indices[counter++] = index2;
                    indices[counter++] = index3;
                }
            }

            var startVertex = vertices[indices[0]].Position;
            var endVertex = vertices[indices[indices.Length - 1]].Position;

            Debug.Print(
                "({0}, {1}) [{2}] -> ({3}, {4})[{5}]",
                startVertex.X, startVertex.Z, calcIndex(0, 0), endVertex.X, endVertex.Z, calcIndex(ParentNode.Size - 1, ParentNode.Size - 1));


            IndexBuffer = new IndexBuffer(Game.GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(indices);

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = new Vector3(0, 0, 0);
            }

            for (var i = 0; i < indices.Length / 3; i++)
            {
                var index1 = indices[i * 3];
                var index2 = indices[i * 3 + 1];
                var index3 = indices[i * 3 + 2];

                var side1 = vertices[index1].Position - vertices[index3].Position;
                var side2 = vertices[index1].Position - vertices[index2].Position;
                var normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.Normalize();
            }
            
            VertexBuffer = new VertexBuffer(Terrain.Game.Graphics.GraphicsDevice, typeof(VertexTerrain), vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);

            //NumVertices = Terrain.Vertices.Count - VertexBufferOffset;
        }

        VertexTerrain[] vertices;

        public void CreateIndices()
        {
            
        }

        public void Dispose()
        {
            IndexBuffer.Dispose();
        }

        public void QueryForRenderChunks(ref RenderPassDescriptor pass)
        {
            var chunk = Game.Graphics.RenderChunkManager.AllocateGeometryChunk();
            chunk.Indices = IndexBuffer;
            chunk.VertexStreams.Add(VertexBuffer);
            chunk.WorldTransform = Matrix.Identity;
            chunk.NumVertices = VertexBuffer.VertexCount;
            chunk.StartIndex = 0;
            chunk.VertexStreamOffset = 0;
            chunk.PrimitiveCount = IndexBuffer.IndexCount / 3;
            chunk.Material = Terrain.Material;
            chunk.Type = PrimitiveType.TriangleList;
        }
    }
}
