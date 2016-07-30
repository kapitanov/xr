using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Configuration
{
    public class ContentSettings
    {
        public string ContentRootPath { get; set; }

        internal static ContentSettings Load(XElement root)
        {
            var settings = new ContentSettings
            {
                ContentRootPath = root.Attribute("contentPath").Value
            };

            return settings;
        }

        internal void Save(XElement root)
        {
            root.Add(new XAttribute("contentPath", ContentRootPath));
        }
    }
}
