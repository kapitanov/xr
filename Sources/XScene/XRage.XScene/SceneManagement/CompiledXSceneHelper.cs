using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Content.SceneManagement;
using System.Globalization;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.SceneManagement
{
    public static class CompiledXSceneHelper
    {
        public static bool HasProperty(this CompiledEntity entity, string propertyName)
        {
            return entity.Properties.ContainsKey(propertyName);
        }

        public static Color ColorProperty(this CompiledEntity entity, string propertyName)
        {
            var parts = entity.Properties[propertyName]
                .Split(' ')
                .Select(str => byte.Parse(str))
                .ToArray();

            if (parts.Length < 3 ||
                parts.Length > 4)
                throw new ArgumentException(string.Format("Value of property \"{0}\" is not a valid Color value. Color must be specified in RGB or RGBA format", propertyName));

            if (parts.Length == 3)
                return new Color(parts[0], parts[1], parts[2]);

            return new Color(parts[0], parts[1], parts[2], parts[3]);
        }

        public static float FloatProperty(this CompiledEntity entity, string propertyName)
        {
            return float.Parse(entity.Properties[propertyName], NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        public static string StringProperty(this CompiledEntity entity, string propertyName)
        {
            if (!entity.HasProperty(propertyName))
                return string.Empty;

            return entity.Properties[propertyName];
        }

        public static Vector3 Vector3Property(this CompiledEntity entity, string propertyName)
        {
            var parts = entity.Properties[propertyName]
                .Split(' ')
                .Select(str => float.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture))
                .ToArray();

            if (parts.Length != 3)
                throw new ArgumentException(string.Format("Value of property \"{0}\" is not a valid Vector3 value.", propertyName));

            return new Vector3(parts[0], parts[1], parts[2]);
        }

        public static LightType LightTypeProperty(this CompiledEntity entity, string propertyName)
        {
            switch (entity.Properties[propertyName])
            {
                case "directional":
                    return LightType.Directional;

                case "point":
                    return LightType.Point;

                case "spot":
                    return LightType.Spot;

                default:
                    throw new ArgumentException(string.Format("Value of property \"{0}\" is not a valid LightType value.", propertyName));
            }
        }

        public static FalloffType FalloffTypeProperty(this CompiledEntity entity, string propertyName)
        {
            switch (entity.Properties[propertyName])
            {
                case "inverseLinear":
                    return FalloffType.InverseLinear;

                case "linear":
                    return FalloffType.Linear;

                default:
                    throw new ArgumentException(string.Format("Value of property \"{0}\" is not a valid FalloffType value.", propertyName));
            }
        }

        public static bool BoolProperty(this CompiledEntity entity, string propertyName)
        {
            var value = entity.Properties[propertyName].ToLowerInvariant();
            if (BooleanTrueValues.Contains(value))
                return true;

            if (BooleanFalseValues.Contains(value))
                return false;

            throw new ArgumentException(string.Format("Value of property \"{0}\" is not a valid Boolean value.", propertyName));
        }

        private static readonly string[] BooleanTrueValues = new[] { "yes", "on", "true" };
        private static readonly string[] BooleanFalseValues = new[] { "no", "off", "false" };
    }
}
