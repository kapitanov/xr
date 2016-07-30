using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.InputManagement
{
    public interface IInputBindingConstructor
    {
        string KnownType { get; }

        InputBinding CreateInputBinding(XElement element);
    }
}
