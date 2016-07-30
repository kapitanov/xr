using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageChildRemoved : Message<BaseEntity>
    {
        public MessageChildRemoved()
            : base((int)MessageType.Entity.ChildRemoved)
        { }

        public BaseEntity Child
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
