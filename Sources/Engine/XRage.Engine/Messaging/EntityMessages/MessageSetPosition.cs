using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageSetPosition : Message<Vector3>
    {
        public MessageSetPosition()
            : base((int)MessageType.Entity.SetPosition)
        { }

        public Vector3 Position
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
