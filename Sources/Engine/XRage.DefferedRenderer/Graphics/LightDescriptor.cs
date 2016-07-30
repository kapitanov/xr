using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    internal sealed class LightDescriptor : IPoolItem
    {
     

        public void Release()
        {
        }

        public void Acquire()
        { }

        public bool IsHandled { get; set; }
    }
}
