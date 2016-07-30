using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageParentAdded : Message<BaseEntity>
    {
        public MessageParentAdded()
            : base((int)MessageType.Entity.ParentAdded)
        { }

        public BaseEntity Parent
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
