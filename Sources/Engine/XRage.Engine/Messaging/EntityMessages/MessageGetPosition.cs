using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageGetPosition : Message<Vector3>
    {
        public MessageGetPosition()
            : base((int)MessageType.Entity.GetPosition)
        { }

        public Vector3 Position
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
