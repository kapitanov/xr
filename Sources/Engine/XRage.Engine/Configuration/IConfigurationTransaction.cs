using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Configuration
{
    internal interface IConfigurationTransaction : IDisposable
    {
        XDocument Root { get; }
    }
}
