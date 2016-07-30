using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using System.Xml.Linq;

namespace AISTek.XRage.Configuration
{
    public class Settings
    {
        public GraphicsSettings GraphicsSettings { get; private set; }

        public PhysicsSettings PhysicsSettings { get; private set; }

        public ContentSettings ContentSettings { get; private set; }

        public GameSettings GameSettings { get; private set; }
                        
        internal static Settings Load(XElement root)
        {
            var settings = new Settings
            {
                GraphicsSettings = GraphicsSettings.Load(root.Descendants("graphics").First()),
                PhysicsSettings = PhysicsSettings.Load(root.Descendants("physics").First()),
                ContentSettings = ContentSettings.Load(root.Descendants("content").First()),
                GameSettings = GameSettings.Load(root.Descendants("game").First())
            };

            return settings;
        }

        internal void Save(XElement root)
        {
            var graphics = new XElement("graphics");
            GraphicsSettings.Save(graphics);

            var physics = new XElement("physics");
            PhysicsSettings.Save(physics);

            var content = new XElement("content");
            ContentSettings.Save(content);

            var game = new XElement("game");
            GameSettings.Save(game);

            root.Add(graphics);
            root.Add(physics);
            root.Add(content);
            root.Add(game);
        }
    }
}
