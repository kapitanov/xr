using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.VmfParsing
{
    public abstract class VmfNode
    {
        protected VmfNode(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
