using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.InputManagement
{
    public interface IInputBindingWriter
    {
        Type KnownType { get; }

        XElement WriteBinding(InputBinding binding);
    }
}
