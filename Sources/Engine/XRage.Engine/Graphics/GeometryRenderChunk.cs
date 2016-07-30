using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Definition of a renderable chunk of geometry handled by the graphics system.
    /// </summary>
    public class GeometryRenderChunk : IRenderChunk
    {
        public GeometryRenderChunk()
        {
            VertexStreams = new List<VertexBuffer>();
        }
        
        /// <summary>
        /// The <see cref="VertexBuffer"/>s used for the geometry chunk.
        /// </summary>
        public List<VertexBuffer> VertexStreams { get; private set; }

        /// <summary>
        /// The <see cref="IndexBuffer"/> used for the geometry chunk.
        /// </summary>
        public IndexBuffer Indices { get; set; }
               
        /// <summary>
        /// The starting index in the index buffer used for the geometry chunk.
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// The number of vertices in the geometry chunk.
        /// </summary>
        public int NumVertices { get; set; }

        /// <summary>
        /// The number of primitives in the geometry chunk.
        /// </summary>
        public int PrimitiveCount { get; set; }

        /// <summary>
        /// The offset into the vertex stream for the geometry chunk.
        /// </summary>
        public int VertexStreamOffset { get; set; }

        /// <summary>
        /// The type of primitive defined in the geometry chunk.
        /// </summary>
        public PrimitiveType Type { get; set; }

        /// <summary>
        /// The world transformation matrix of the geometry chunk.
        /// </summary>
        public Matrix WorldTransform { get; set; }

        /// <summary>
        /// The material assigned to the geometry chunk.
        /// </summary>
        public Material Material { get; set; }

        public float Opacity { get; set; }
        
        /// <summary>
        /// The rendering order of the chunk.
        /// </summary>
        public PreferredRenderOrder RenderOrder { get; set; }

        /// <summary>
        /// Recycles and prepares the <see cref="GeometryChunk"/> the reallocation.
        /// </summary>
        public void Recycle()
        {
            Indices = null;
            VertexStreams.Clear();
            Material = null;
            WorldTransform = Matrix.Identity;
        }
    }
}
