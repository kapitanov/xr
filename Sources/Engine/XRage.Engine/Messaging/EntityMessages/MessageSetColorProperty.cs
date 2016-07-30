using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageSetColorProperty : MessageSetProperty<Color>
    {
        public MessageSetColorProperty()
            : base((int)MessageType.Entity.SetColorProperty)
        { }
    }
}
