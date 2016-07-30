using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Linq;

namespace AISTek.XRage.Sample.GameMenu
{
    public class GameMenuItem : XComponent
    {
        public GameMenuItem(GameMenuScreen menu, XElement node)
            : base(menu.Game)
        {
            Menu = menu;
            Title = node.Attribute("title").Value;
            Action = Menu.ActionFactory.CreateMenuAction(
                node.Descendants("item.action").Descendants().First(), 
                Game, 
                this);
        }

        public string Title { get; private set; }

        public bool IsSelected { get; set; }

        public GameMenuScreen Menu { get; set; }

        public BaseMenuAction Action { get; private set; }

        internal void Draw(GameTime gameTime, int itemIndex)
        {
            var position =new Vector2(256, 128 + itemIndex * 32);
            if (IsSelected)
            {
                Game.Graphics.SpriteBatch.DrawString(Menu.MenuFont, string.Format("> {0} <", Title), position, Color.Green);
            }
            else
            {
                Game.Graphics.SpriteBatch.DrawString(Menu.MenuFont, string.Format("  {0}", Title), position, Color.White);
            }                        
        }        
    }
}
