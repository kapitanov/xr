using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics.Profiling
{
    internal interface IPixDriver
    {
        void DisablePixProfiling();

        void BeginEvent(string eventName);

        void EndEvent();

        void SetMarker(string markerName);
    }
}
