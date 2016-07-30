using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Configuration
{
    internal class ConfigurationTransaction : IConfigurationTransaction
    {
        public ConfigurationTransaction(string fileName)
        {
            this.fileName = fileName;
            Root = new XDocument();
        }

        public XDocument Root { get; private set; }

        public void Dispose()
        {
            Root.Save(fileName);
        }

        private string fileName;
    }
}
