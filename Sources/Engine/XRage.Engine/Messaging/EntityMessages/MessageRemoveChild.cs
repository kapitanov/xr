using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageRemoveChild : Message<BaseEntity>
    {
        public MessageRemoveChild()
            : base((int)MessageType.Entity.RemoveChild)
        { }

        public BaseEntity Child
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
