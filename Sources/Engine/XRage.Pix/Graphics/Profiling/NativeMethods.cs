using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AISTek.XRage.Graphics.Profiling
{
    internal static class NativeMethods
    {
        [DllImport("d3d9.dll")]
        public static extern uint D3DPERF_GetStatus();

        [DllImport("d3d9.dll")]
        public static extern void D3DPERF_SetOptions(uint dwOptions);

        [DllImport("d3d9.dll", CharSet = CharSet.Unicode)]
        public static extern int D3DPERF_BeginEvent(uint col, string wszName);

        [DllImport("d3d9.dll")]
        public static extern int D3DPERF_EndEvent();

        [DllImport("d3d9.dll", CharSet = CharSet.Unicode)]
        public static extern void D3DPERF_SetMarker(uint col, string wszName);
    }
}
