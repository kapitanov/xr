using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AISTek.XRage.Entities
{
    public class FreeLookPerspectiveCamera : BasePerspectiveCamera
    {
        public FreeLookPerspectiveCamera (XGame game)
            : base(game, "free_look_camera", null)
        { }

        public override void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //    MoveView(new Vector3(0f, 0f, 0.25f));
            //if (Keyboard.GetState().IsKeyDown(Keys.S))
            //    MoveView(new Vector3(0f, 0f, -0.25f));

            //if (Keyboard.GetState().IsKeyDown(Keys.R))
            //    RotateCameraView(new CameraRotation { Yaw = 10 });
            //if (Keyboard.GetState().IsKeyDown(Keys.F))
            //    RotateCameraView(new CameraRotation { Yaw = -10 });


            base.Update(gameTime);
        }

        public override string GetCameraInfo()
        {
            return string.Format("free_look_perspective_camera: {0}", Position);
        }
    }
}
