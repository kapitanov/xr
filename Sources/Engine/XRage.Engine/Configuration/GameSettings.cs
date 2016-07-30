using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

namespace AISTek.XRage.Configuration
{
    public class GameSettings
    {
        public float MouseSensitivity { get; set; }

        internal static GameSettings Load(XElement root)
        {
            var mouseSensitivity = float.Parse(
                root.Attribute("mouseSensitivity").Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture);

            var settings = new GameSettings
            {
                MouseSensitivity = mouseSensitivity
            };

            return settings;
        }

        internal void Save(XElement root)
        {
            root.Add(new XAttribute("mouseSensitivity", MouseSensitivity));
        }
    }
}
