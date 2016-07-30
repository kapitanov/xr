using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AISTek.XRage.Content.VmfParsing
{
    public class VmfParser
    {
        public VmfDocument ReadVmf(string vmfContent)
        {
            var tokens = tokenStreamParser.ReadTokens(vmfContent);
            var tree = treeBuilder.BuildTree(tokens);
            return tree;
        }

        private VmfTokenStreamParser tokenStreamParser = new VmfTokenStreamParser();
        private VmfTreeBuilder treeBuilder = new VmfTreeBuilder();
    }
}
