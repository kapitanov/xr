using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Sample.GameMenu
{
    public interface IBaseMenuActionConstructor
    {
        string KnownType { get; }

        BaseMenuAction CreateMenuAction(XElement element, XGame game, GameMenuItem menuItem);
    }
}
