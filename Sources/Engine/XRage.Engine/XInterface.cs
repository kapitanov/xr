using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Messaging;

namespace AISTek.XRage
{
   public abstract  class XInterface : XComponent
    {
       protected XInterface(XGame game, InterfaceType interfaceType)
           : base(game)
       {
           InterfaceType = interfaceType;
       }

       public InterfaceType InterfaceType { get; private set; }

       /// <summary>
       /// Updates interface.
       /// </summary>
       /// <param name="gameTime">
       /// Contains timer information
       /// </param>
       public virtual void Update(GameTime gameTime)
       { }

       /// <summary>
       /// Sends a message directly to an interface.
       /// </summary>
       /// <param name="message">
       /// Incoming message
       /// </param>
       /// <returns>
       /// Whether or not the message was handled by the interface
       /// </returns>
       public virtual bool ExecuteMessage(IMessage message)
       {
           return false;
       }

       public virtual void Shutdown()
       { }
    }
}
