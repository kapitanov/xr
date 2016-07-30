using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Entities
{
    public class CameraRotation
    {
        public float Pitch
        {
            get { return pitch; }
            set { pitch = MathHelper.Clamp(value, MinAngle, MaxAngle); }
        }

        public float Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }

        public float Roll
        {
            get { return roll; }
            set { roll = MathHelper.Clamp(value, MinAngle, MaxAngle); }
        }

        private float pitch;
        private float yaw;
        private float roll;

        private static readonly float MaxAngle = MathHelper.PiOver2 - MathHelper.ToRadians(0.1f);
        private static readonly float MinAngle = -MaxAngle;

        public override string ToString()
        {
            return string.Format("{{P: {0} Y: {1} R: {2}}}", Pitch, Yaw, Roll);
        }
    }
}