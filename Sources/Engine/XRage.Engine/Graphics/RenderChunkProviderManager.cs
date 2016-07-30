using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    public class RenderChunkProviderManager : XComponent
    {
        public RenderChunkProviderManager(XGame game)
            : base(game)
        { }

        /// <summary>
        /// Inserts an IRenderChunkProvider instance into the per-frame processing list.  All registered IRenderChunkProviders
        /// will be queried for every rendering frame every frame.
        /// </summary>
        /// <param name="provider">
        /// The IRenderChunkProvider to register.
        /// </param>
        public void RegisterRenderChunkProvider(IRenderChunkProvider provider)
        {
            renderChunkProviders.Add(provider);
        }

        /// <summary>
        /// Removes an IRenderChunkProvider instance from the per-frame processing list.
        /// </summary>
        /// <param name="provider">
        /// The IRenderChunkProvider to remove.
        /// </param>
        public void UnregisterRenderChunkProvider(IRenderChunkProvider provider)
        {
            renderChunkProviders.Remove(provider);
        }

         /// <summary>
        /// Query the system for potentially visible renderable chunks.
        /// </summary>
        /// <param name="pass">
        /// A descriptor for the rendering pass.
        /// </param>
        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            foreach (var provider in renderChunkProviders)
            {
                provider.QueryForChunks(ref pass);
            }
        }

        private List<IRenderChunkProvider> renderChunkProviders = new List<IRenderChunkProvider>();
    }
}
