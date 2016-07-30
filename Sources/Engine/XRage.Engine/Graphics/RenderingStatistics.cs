using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    public class RenderingStatistics : XComponent
    {
        public RenderingStatistics(XGame game)
            : base(game)
        { }

        public long PrimitivesDraw { get; private set; }

        public long GeometryChunksDrawn { get; private set; }

        public long LightChunksDrawn { get; private set; }

        public void IncrementPrimitivesCount(int primitivesCount)
        {
            PrimitivesDraw += primitivesCount;
        }

        public void IncrementGeometryChunksCount(int geometryChunksCount)
        {
            GeometryChunksDrawn += geometryChunksCount;
        }

        public void IncrementLightChunksCount(int lightChunksCount)
        {
            LightChunksDrawn += lightChunksCount;
        }
        
        public void Clear()
        {
            PrimitivesDraw = 0;
            GeometryChunksDrawn = 0;
            LightChunksDrawn = 0;
        }
    }
}
