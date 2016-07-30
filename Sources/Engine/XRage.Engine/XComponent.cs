using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage
{
    public abstract class XComponent
    {
        protected XComponent(XGame game)
        {
            Game= game;
        }

        public XGame Game { get; private set; }
    }
}
