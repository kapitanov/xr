using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageModifyPosition : Message<Vector3>
    {
        public MessageModifyPosition()
            : base((int)MessageType.Entity.ModifyPosition)
        { }

        public Vector3 Position
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
