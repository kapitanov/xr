using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AISTek.XRage.Content.VmfParsing
{
    [DebuggerDisplay("{Type} {Value}")]
    internal class VmfToken : IEquatable<VmfToken>
    {
        private VmfToken(string value, VmfTokenType type)
        {
            Value = value;
            Type = type;
        }

        public static VmfToken ClassOpening()
        {
            return new VmfToken("{", VmfTokenType.ClassOpening);
        }

        public static VmfToken ClassClosing()
        {
            return new VmfToken("}", VmfTokenType.ClassClosing);
        }

        public static VmfToken ClassName(string name)
        {
            return new VmfToken(name, VmfTokenType.ClassName);
        }

        public static VmfToken PropertyName(string name)
        {
            return new VmfToken(name, VmfTokenType.PropertyName);
        }

        public static VmfToken PropertyValue(string value)
        {
            return new VmfToken(value, VmfTokenType.PropertyValue);
        }

        public string Value { get; private set; }

        public VmfTokenType Type { get; private set; }

        public bool Equals(VmfToken other)
        {
          return (Value == other.Value) &&
                 (Type == other.Type);
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", Type, Value);
        }
    }
}
