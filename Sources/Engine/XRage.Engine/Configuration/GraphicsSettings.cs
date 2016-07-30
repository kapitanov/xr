using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AISTek.XRage.Configuration;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Configuration
{
    /// <summary>
    /// Defines configurable settings applied to a graphics device.
    /// </summary>
    public class GraphicsSettings
    {
        /// <summary>
        /// The width of the back buffer, in pixels.
        /// </summary>
        public int BackBufferWidth { get; set; }

        /// <summary>
        /// The height of the back buffer, in pixels.
        /// </summary>
        public int BackBufferHeight { get; set; }
      
        /// <summary>
        /// Flag enabling/disabling full-screen rendering.  Ignored on Xbox 360 platform.
        /// </summary>
        public bool IsFullScreen { get; set; }

        public bool SynchronizeWithVerticalRetrace { get; set; }

        public ConfigurationNode RendererSettings { get; set; }

        internal static GraphicsSettings Load(XElement root)
        {
            var mode = root.Descendants("mode").First();
            var renderer = root.Descendants("renderer").First();

            var settings = new GraphicsSettings 
            {
                 BackBufferWidth = mode.ReadIntAttribute("width"),
                 BackBufferHeight =mode.ReadIntAttribute("height"),
                 SynchronizeWithVerticalRetrace = mode.ReadBoolAttribute("vsynch"),
                 IsFullScreen = mode.ReadBoolAttribute("fullscreen"),
                 RendererSettings = new ConfigurationNode(renderer)
            };
            return settings;
        }

        internal void Save(XElement root)
        {
            root.Add(
                new XElement("mode", 
                    new XAttribute("width", BackBufferWidth),
                    new XAttribute("height", BackBufferHeight),
                    new XAttribute("vsynch", ConfigurationStorageHelper.ConvertBool(SynchronizeWithVerticalRetrace)),
                    new XAttribute("fullscreen", ConfigurationStorageHelper.ConvertBool(IsFullScreen))),
                    RendererSettings.ToXml());
        }
    }
}
