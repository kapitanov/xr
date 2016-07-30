using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

namespace AISTek.XRage.Content
{
    internal static class XmlDataHelper
    {
        public static bool ReadBoolAttribute(this XElement element, string attributeName, bool defaultValue = false)
        {
            var attribute = element.Attributes(attributeName).FirstOrDefault();
            if (attribute == null)
                return defaultValue;

            var text = attribute.Value.ToLowerInvariant();
            return (text == "on") ||
                   (text == "true") ||
                   (text == "yes");
        }

        public static float ReadFloatAttribute(this XElement element, string attributeName, float defaultValue = 0.0f)
        {
            var attribute = element.Attributes(attributeName).FirstOrDefault();
            if (attribute == null)
                return defaultValue;

            var value = attribute.Value.ToLowerInvariant();
            return float.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
