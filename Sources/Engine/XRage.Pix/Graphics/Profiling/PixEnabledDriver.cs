using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics.Profiling
{
    internal sealed class PixEnabledDriver : IPixDriver
    {
        public void DisablePixProfiling()
        {
            NativeMethods.D3DPERF_SetOptions(1);
        }

        public void BeginEvent(string eventName)
        {
            NativeMethods.D3DPERF_BeginEvent(Color.Black.PackedValue, eventName);
        }

        public void EndEvent()
        {
            NativeMethods.D3DPERF_EndEvent();
        }

        public void SetMarker(string markerName)
        {
            NativeMethods.D3DPERF_SetMarker(Color.Black.PackedValue, markerName);
        }
    }
}
