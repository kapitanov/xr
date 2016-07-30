using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AISTek.XRage.Content.VmfParsing
{
    public static class VmfPropertyValueHelper
    {
        public static Vector3 ReadVector3(this VmfClassNode node, string name)
        {
            try
            {

                var attr = node.Children.OfType<VmfPropertyNode>()
                                        .FirstOrDefault(a => a.Name == name);
                if (attr == null)
                    return Vector3.Zero;

                var match = vector3Regex.Match(attr.Value);
                return new Vector3(
                    float.Parse(match.Groups["n1"].Value),
                    float.Parse(match.Groups["n2"].Value),
                    float.Parse(match.Groups["n3"].Value));
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public static Vector3[] ReadVertices(this VmfClassNode node, string name)
        {
            var attr = node.Children.OfType<VmfPropertyNode>()
                                               .FirstOrDefault(a => a.Name == name);
            if (attr == null)
                return new Vector3[0];

            var match = verticesRegex.Match(attr.Value);

            try
            {
                return new[] 
                {
                    match.Groups["v1"].Value, 
                    match.Groups["v2"].Value, 
                    match.Groups["v3"].Value 
                }
                    .Select(x => vector3Regex.Match(x))
                    .Select(m => new[] 
                    { 
                        m.Groups["n1"].Value, 
                        m.Groups["n2"].Value, 
                        m.Groups["n3"].Value 
                    })
                    .Select(x => new Vector3(
                        float.Parse(x[0], NumberStyles.Any, CultureInfo.InvariantCulture),
                        float.Parse(x[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                        float.Parse(x[2], NumberStyles.Any, CultureInfo.InvariantCulture)))
                    .ToArray();
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private static Regex vector3Regex = new Regex(@"(?<n1>(-|)[0-9.,e\+\-]+) (?<n2>(-|)[0-9.,e\+\-]+) (?<n3>(-|)[0-9.,e\+\-]+)");
        private static Regex verticesRegex = new Regex(@"\((?<v1>[^\)]+)\)\s*\((?<v2>[^\)]+)\)\s*\((?<v3>[^\)]+)\)");
    }
}
