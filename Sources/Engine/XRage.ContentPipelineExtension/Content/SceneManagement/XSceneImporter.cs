using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Linq;

namespace AISTek.XRage.Content.SceneManagement
{
    [ContentImporter(".xscene", DisplayName = "XScene importer")]
    public class XSceneImporter : ContentImporter<XDocument>
    {
        public override XDocument Import(string filename, ContentImporterContext context)
        {
            return XDocument.Load(filename);
        }
    }
}
