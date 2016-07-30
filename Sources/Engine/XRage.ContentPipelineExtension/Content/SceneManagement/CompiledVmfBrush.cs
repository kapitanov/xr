using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AISTek.XRage.Content.SceneManagement
{
    [DebuggerDisplay("Brush {Sides.Count} faces, material {MaterialPath}")]
    public class CompiledVmfBrush
    {
        public CompiledVmfBrush()
        {
            Sides = new List<VmfBrushSide>();

            MaterialPath = string.Empty;
        }

        public string MaterialPath { get; set; }

        public List<VmfBrushSide> Sides { get;  set; }
    }
}
