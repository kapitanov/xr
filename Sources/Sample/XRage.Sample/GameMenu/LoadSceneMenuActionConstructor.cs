using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Sample.GameMenu
{
    public class LoadSceneMenuActionConstructor : IBaseMenuActionConstructor
    {
        public string KnownType { get { return "load_scene"; } }

        public BaseMenuAction CreateMenuAction(XElement element, XGame game, GameMenuItem menuItem)
        {
            return new LoadSceneMenuAction(game, menuItem, element.Attribute("path").Value);
        }
    }
}
