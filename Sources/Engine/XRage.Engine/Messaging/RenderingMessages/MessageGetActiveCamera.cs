using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.RenderingMessages
{
    public class MessageGetActiveCamera : Message<BaseCamera>
    {
        public MessageGetActiveCamera ()
            : base((int)MessageType.Renderer.GetActiveCamera)
        { }

        public BaseCamera Camera
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
