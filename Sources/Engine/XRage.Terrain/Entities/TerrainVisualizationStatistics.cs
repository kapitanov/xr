using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Entities
{
    internal class TerrainVisualizationStatistics
    {
        public int NodesVisualized { get; private set; }

        public void NodeVisualized()
        {
            NodesVisualized++;
        }

        public void ClearStatistics()
        {
            NodesVisualized = 0;
        }
    }
}
