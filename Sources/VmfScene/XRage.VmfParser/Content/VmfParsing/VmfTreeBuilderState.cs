using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.VmfParsing
{
    internal enum VmfTreeBuilderState
    {
        Title,
        Start,
        Content,
        Attribute,
        End
    }
}
