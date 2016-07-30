using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.RenderingMessages
{
    public class MessageRotateCamera : Message<CameraRotation>
    {
        public MessageRotateCamera ()
            : base((int)MessageType.Renderer.RotateCamera)
        { }

        public float Pitch
        {
            get { return Data.Pitch; }
            set { Data.Pitch = value; }
        }

        public float Yaw
        {
            get { return Data.Yaw; }
            set { Data.Yaw = value; }
        }

        public float Roll
        {
            get { return Data.Roll; }
            set { Data.Roll = value; }
        }
    }
}
