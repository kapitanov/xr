using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageGetName : Message<string>
    {
        public MessageGetName()
            : base((int)MessageType.Entity.GetName)
        { }

        public string Name
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
