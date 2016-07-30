using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Messaging;
using AISTek.XRage.Messaging.RenderingMessages;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Interfaces
{
    public class CameraInterface : XInterface
    {
        public CameraInterface (XGame game)
            : base(game, InterfaceType.Camera)
        {
            Game.MessagingPoll.OnMessage += OnGameMessage;
        }

        public BaseCamera ActiveCamera { get; set; }

        public override bool ExecuteMessage(IMessage message)
        {
            switch(message.Type)
            {
                case (int)MessageType.Renderer.GetActiveCamera:
                    message.TypeCheck<MessageGetActiveCamera>().Camera = ActiveCamera;
                    return true;

                case (int)MessageType.Renderer.SetActiveCamera:
                    ActiveCamera = message.TypeCheck<MessageSetActiveCamera>().Camera;
                    return true;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (ActiveCamera != null)
            {
                ActiveCamera.Update(gameTime);
            }
        }

        public override void Shutdown()
        {
            Game.MessagingPoll.OnMessage -= OnGameMessage;
            ActiveCamera = null;
        }

        private void OnGameMessage(IMessage message)
        {
            ExecuteMessage(message);
        }
    }
}
