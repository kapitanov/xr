using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Configuration
{
    internal static class ConfigurationStorageHelper
    {
        internal static bool ReadBoolAttribute(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName).Value.Equals("on", StringComparison.InvariantCultureIgnoreCase);
        }

        internal static int ReadIntAttribute(this XElement element, string attributeName)
        {
            return int.Parse(element.Attribute(attributeName).Value);
        }

        internal static T ReadEnumAttribute<T>(this XElement element, string attributeName)
            where T: struct
        {
            var value = element.Attribute(attributeName).Value;
            T result;

            if (!Enum.TryParse<T>(value, out result))
            {
                throw new ArgumentException(string.Format("value \"{0}\" is not a member of enum \"{1}\"", value, typeof(T).Name));
            }

            return result;
        }

        internal static Type ReadTypeAttribute(this XElement element, string attributeName)
        {
            return Type.GetType(element.Attribute(attributeName).Value);
        }

        internal static string ConvertBool(bool value)
        {
            return value ? "on" : "off";
        }

        internal static string ConvertEnum<T>(T value)
            where T : struct
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}
