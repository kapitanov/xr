using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageSetRotation : Message<Matrix>
    {
        public MessageSetRotation()
            : base((int)MessageType.Entity.SetRotation)
        { }

        public Matrix Rotation
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
