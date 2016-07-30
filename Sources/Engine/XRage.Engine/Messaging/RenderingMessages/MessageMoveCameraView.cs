using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.RenderingMessages
{
    public class MessageMoveCameraView : Message<Vector3>
    {
        public MessageMoveCameraView()
            : base((int)MessageType.Renderer.MoveCameraView)
        { }

        public Vector3 Movement
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
