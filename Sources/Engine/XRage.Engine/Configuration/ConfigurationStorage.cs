using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Configuration
{
    public class ConfigurationStorage : XComponent
    {
        public ConfigurationStorage(XGame game)
            : base(game)
        {
            SettingsFilePath = "./conf/settings.xml";
            InputConfigurationFilePath= "./conf/input.xml";
        }

        public string SettingsFilePath { get; set; }

         public string InputConfigurationFilePath { get; set; }

        public Settings LoadSettings()
        {
            var xdoc = XDocument.Load(SettingsFilePath);
            var root = xdoc.Descendants("settings").First();

            return Settings.Load(root);
        }

        public void SaveSettings(Settings settings)
        {
            var root = new XElement("settings");
            var doc = new XDocument(root);

            settings.Save(root);
            doc.Save(SettingsFilePath);
        }

        internal XDocument LoadInputConfiguration()
        {
            return XDocument.Load(InputConfigurationFilePath);
        }

        internal IConfigurationTransaction SaveInputConfiguration()
        {
            return new ConfigurationTransaction(InputConfigurationFilePath);
        }
    }
}
