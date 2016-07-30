using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageParentRemoved : Message<BaseEntity>
    {
        public MessageParentRemoved()
            : base((int)MessageType.Entity.ParentRemoved)
        { }

        public BaseEntity Parent
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
