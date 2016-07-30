using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public class MessageSetName : Message<string>
    {
        public MessageSetName()
            : base((int)MessageType.Entity.SetName)
        { }

        public string Name
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
