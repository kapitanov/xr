using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Entities
{
    internal class QuadTree : IDisposable
    {
        private QuadTree(Terrain terrain)
        {
            Terrain = terrain;
            Size = terrain.Size;
            OffsetX = 0;
            OffsetZ = 0;
        }

        public QuadTree(Terrain terrain, int size, int offsetX, int offsetZ)
            : this(terrain)
        {
            Size = size;
            OffsetX = offsetX;
            OffsetZ = offsetZ;

           CreateBoundingBox();

            if (Size < Terrain.MinimumLeafSize)
            {
                // Create leaf
                LeafPatch = new TerrainPatch(this);
               
            }
            else
            {
                // Create branches
                BranchOff();
            }
        }

        public QuadTree(Terrain terrain, int size)
            : this(terrain, size, 0, 0)
        { }

        public Terrain Terrain { get; private set; }

        public int Size { get; private set; }

        public int OffsetX { get; private set; }

        public int OffsetZ { get; private set; }

        public TerrainPatch LeafPatch { get; private set; }

        public QuadTree[] ChildNodes { get; private set; }

        public bool IsLeaf { get { return ChildNodes == null; } }

        public BoundingBox BoundingBox { get; private set; }

        public BoundingBoxMesh BoundingBoxMesh { get; private set; }

        public void Dispose()
        {
            if (IsLeaf)
            {
                LeafPatch.Dispose();
            }
            else
            {
                foreach (var node in ChildNodes)
                {
                    node.Dispose();
                }
            }
        }

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            BoundingBoxMesh.IsVisible = false;
            if (pass.RenderCamera.BoundingFrustum.Intersects(BoundingBox))
            {   
                BoundingBoxMesh.IsVisible = true;

                if (IsLeaf)
                {
                    LeafPatch.QueryForRenderChunks(ref pass);
                    Terrain.Statistics.NodeVisualized();   
                }
                else
                {
                    foreach (var node in ChildNodes)
                    {
                        node.QueryForChunks(ref pass);
                    }
                }
            }

            BoundingBoxMesh.QueryForChunks(ref pass);
        }

        private void CreateBoundingBox()
        {
            var halfSize = Size / 2;
            var firstCorner = new Vector3(
                Terrain.ScaleFactor * (-halfSize + OffsetX),
                Terrain.MinimumElevation,
                Terrain.ScaleFactor * (-halfSize + OffsetZ));
            var lastCorner = new Vector3(
                Terrain.ScaleFactor * (halfSize + OffsetX),
                Terrain.MaximumElevation,
                Terrain.ScaleFactor * (halfSize + OffsetZ));

            BoundingBox = new BoundingBox(firstCorner, lastCorner);
            BoundingBoxMesh = new BoundingBoxMesh(this);
        }

        private void BranchOff()
        {
            var halfSize = Size / 2;
            var quadSize = Size / 4;

            ChildNodes = new QuadTree[4];
            ChildNodes[0] = new QuadTree(Terrain, halfSize, OffsetX + quadSize, OffsetZ + quadSize);
            ChildNodes[1] = new QuadTree(Terrain, halfSize, OffsetX + quadSize, OffsetZ - quadSize);
            ChildNodes[2] = new QuadTree(Terrain, halfSize, OffsetX - quadSize, OffsetZ + quadSize);
            ChildNodes[3] = new QuadTree(Terrain, halfSize, OffsetX - quadSize, OffsetZ - quadSize);
        }
    }
}
