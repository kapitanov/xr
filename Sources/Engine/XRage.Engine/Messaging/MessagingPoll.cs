using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Messaging
{
    public class MessagingPoll : XComponent
    {
        public MessagingPoll(XGame game)
            : base(game)
        { }

        public void SendMessage(IMessage message)
        {
            // TODO: implement messaging
        }

        public void SendInterfaceMessage(InterfaceType type, IMessage message)
        {
            Game.Interfaces.QueryInterface(type).ExecuteMessage(message);
        }

        public event EngineMessageHandler OnMessage;
    }
}
