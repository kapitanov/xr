using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Messaging
{
    /// <summary>
    /// Defines the core message types
    /// </summary>
    /// <remarks>
    /// Core engine messages are in the range of 0 to 100000, please use another range for custom messages
    /// <see cref="int.MinValue"/> denotes a unknown message
    /// </remarks>
    public static class MessageType
    {
        /// <summary>
        /// Unknown message
        /// </summary>
        public const int Unknown = int.MinValue;

        #region Core messages (id range from 0 to 2999)

        #endregion

        #region Input messages (id range from 3000 to 4999)

        #endregion

        #region Rendering messages (id range from 5000 to 6999)

        public enum Renderer
        {
            GetActiveCamera = 5000,
            SetActiveCamera,
            SetCameraPosition,
            MoveCamera,
            MoveCameraView,
            RotateCamera
        }

        #endregion

        #region Entity messages (id range from 7000 to 8999)

        public enum Entity
        {
            ParentAdded = 7000,
            ParentRemoved,
            ChildAdded,
            ChildRemoved ,
            GetName,
            SetName,
            GetParent,
            GetPosition,
            ModifyPosition,
            SetPosition,
            GetRotation,
            ModifyRotation,
            SetRotation,
            SetParent,
            RemoveChild,
            SetColorProperty
        }

        #endregion
    }
}
