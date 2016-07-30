using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Sample.GameMenu
{
    public class ExitGameMenuActionConstructor : IBaseMenuActionConstructor
    {
        public string KnownType { get { return "exit_game"; } }

        public BaseMenuAction CreateMenuAction(XElement element, XGame game, GameMenuItem menuItem)
        {
            return new ExitGameMenuAction(game, menuItem);
        }
    }
}
