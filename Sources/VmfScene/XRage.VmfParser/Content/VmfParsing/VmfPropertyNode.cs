using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AISTek.XRage.Content.VmfParsing
{
    [DebuggerDisplay("VMF_ATTRIBUTE {Name} = {Value} ")]
    public class VmfPropertyNode : VmfNode
    {
        public VmfPropertyNode (string name, string value)
            : base(name)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
