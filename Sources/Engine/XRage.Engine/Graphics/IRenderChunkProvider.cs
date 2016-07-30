using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
     /// <summary>
    /// Interface for systems that provide render chunks for the graphics system.
    /// </summary>
    public interface IRenderChunkProvider
    {
        /// <summary>
        /// Query the system for potentially visible renderable chunks.
        /// </summary>
        /// <param name="pass">A descriptor for the rendering pass.</param>
        void QueryForChunks(ref RenderPassDescriptor pass);
    }
}
