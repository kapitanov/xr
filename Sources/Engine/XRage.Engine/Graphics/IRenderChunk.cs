using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    public interface IRenderChunk
    {
        /// <summary>
        /// Recycles and prepares the <see cref="IRenderChunk"/> the reallocation.
        /// </summary>
        void Recycle();
    }
}
