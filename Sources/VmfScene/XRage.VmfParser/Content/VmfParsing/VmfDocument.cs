using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AISTek.XRage.Content.VmfParsing
{
    [DebuggerDisplay("VMF_ROOT {Children.Count} child nodes")]
    public class VmfDocument : VmfClassNode
    {
        public VmfDocument(IEnumerable<VmfNode> childNodes)
            : base(string.Empty, childNodes)
        { }

        public VmfDocument(params VmfNode[] childNodes)
            : base(string.Empty, childNodes)
        { }
    }
}
