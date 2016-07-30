using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.RenderingMessages
{
    public class MessageSetCameraPosition : Message<Vector3>
    {
        public MessageSetCameraPosition()
            : base((int)MessageType.Renderer.SetCameraPosition)
        { }

        public Vector3 Position
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
