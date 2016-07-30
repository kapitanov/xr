using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageSetParent : Message<BaseEntity>
    {
        public MessageSetParent ()
            : base((int)MessageType.Entity.SetParent)
        {}
        
        public BaseEntity ParentEntity
        {
            get { return Data; }
            set { Data = value; }
        }
    }

}
