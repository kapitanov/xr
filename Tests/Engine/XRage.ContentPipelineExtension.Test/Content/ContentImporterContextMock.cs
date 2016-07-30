using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Diagnostics;

namespace AISTek.XRage.Content
{
    internal class ContentImporterContextMock : ContentImporterContext
    {
        public override void AddDependency(string filename)
        {
            Debug.Print("Added dependency: {0}", filename);
        }

        public override string IntermediateDirectory { get { return "c:\temp"; } }

        public override ContentBuildLogger Logger { get { return logger; } }

        public override string OutputDirectory { get { return "c:\temp"; } }

        private ContentBuildLogger logger = new ContentBuildLoggerMock();
    }
}
