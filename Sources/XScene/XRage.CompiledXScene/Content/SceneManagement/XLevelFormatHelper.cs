using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Linq;
using System.Globalization;

namespace AISTek.XRage.Content.SceneManagement
{
    public static class XLevelFormatHelper
    {
        public static Vector3 ReadVector3(this XElement element)
        {
            return new Vector3(
                element.Attribute("x").ReadFloatAttribute(),
                element.Attribute("y").ReadFloatAttribute(),
                element.Attribute("z").ReadFloatAttribute());
        }

        public static Vector2 ReadVector2(this XElement element)
        {
            return new Vector2(
                element.Attribute("x").ReadFloatAttribute(),
                element.Attribute("y").ReadFloatAttribute());
        }

        private static float ReadFloatAttribute(this XAttribute attribute)
        {
            return float.Parse(attribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
