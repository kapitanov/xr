using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageChildAdded : Message<BaseEntity>
    {
        public MessageChildAdded()
            : base((int)MessageType.Entity.ChildAdded)
        { }

        public BaseEntity Child
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
