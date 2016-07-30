using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.RenderingMessages
{
    public class MessageMoveCamera : Message<Vector3>
    {
        public MessageMoveCamera()
            : base((int)MessageType.Renderer.MoveCamera)
        { }

        public Vector3 Movement
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
