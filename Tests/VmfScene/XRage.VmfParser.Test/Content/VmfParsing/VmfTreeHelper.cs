using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.VmfParsing
{
    internal static class VmfTreeHelper
    {
        public static bool IsEqual(this VmfDocument x, VmfDocument y)
        {
            return (x as VmfClassNode).IsEqual(y as VmfClassNode);
        }

        public static bool IsEqual(this VmfClassNode x, VmfClassNode y)
        {
            if (x.Name != y.Name)
                return false;

            if (x.Children.Count != y.Children.Count)
                return false;

            foreach (var pair in x.Children.Zip(y.Children, (a, b) => new { X = a, Y = b }))
            {
                var type = pair.X.GetType();
                if (pair.Y.GetType() != type)
                    return false;

                if (type == typeof(VmfClassNode))
                {
                    if (!(pair.X as VmfClassNode).IsEqual(pair.Y as VmfClassNode))
                    {
                        return false;
                    }
                }
                else if (type == typeof(VmfPropertyNode))
                {
                    if (!(pair.X as VmfPropertyNode).IsEqual(pair.Y as VmfPropertyNode))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEqual(this VmfPropertyNode x, VmfPropertyNode y)
        {
            if (x.Name != y.Name)
                return false;

            return x.Value == y.Value;
        }
    }
}
