using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.RenderingMessages
{
    public class MessageSetActiveCamera : Message<BaseCamera>
    {
        public MessageSetActiveCamera()
            : base((int)MessageType.Renderer.SetActiveCamera)
        { }

        public BaseCamera Camera
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
