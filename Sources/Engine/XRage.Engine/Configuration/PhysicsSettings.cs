using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Configuration
{
    public class PhysicsSettings
    {
        public string PhysicsManagerFactory { get; set; }

        internal static PhysicsSettings Load(XElement root)
        {
            var settings = new PhysicsSettings
            {
                PhysicsManagerFactory = root.Attribute("factory").Value
            };

            return settings;
        }

        internal void Save(XElement root)
        {
            root.Add(new XAttribute("factory", PhysicsManagerFactory));
        }
    }
}
