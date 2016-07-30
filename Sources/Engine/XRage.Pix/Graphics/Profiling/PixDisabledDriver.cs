using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace AISTek.XRage.Graphics.Profiling
{
    internal sealed class PixDisabledDriver : IPixDriver
    {
        public void DisablePixProfiling()
        { }

        public void BeginEvent(string eventName)
        { }

        public void EndEvent()
        { }

        public void SetMarker(string markerName)
        { }

        /// <summary>
        /// Keeps track of our BeginEvent/EndEvent pairs
        /// </summary>
        private static int eventCount = 0;
    }
}
