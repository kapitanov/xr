using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AISTek.XRage.Content.VmfParsing
{
    [DebuggerDisplay("VMF_SECTION {Name} {Children.Count} child nodes")]
    public class VmfClassNode : VmfNode
    {
        public VmfClassNode(string name, IEnumerable<VmfNode> childNodes)
            : base(name)
        {
            Children = new VmfNodeCollection(childNodes);
        }

        public VmfClassNode(string name, params VmfNode[] childNodes)
            : this(name, (IEnumerable<VmfNode>)childNodes)
        { }

        public VmfNodeCollection Children { get; private set; }
    }
}
