using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging
{
    public abstract class Message<T> : IMessage
    {
        protected Message(int type)
        {
            this.type = type;
            (this as IPoolItem).Release();
            IsHandled = false;
        }

        public GameTime Time { get; set; }

        public T Data { get; set; }

        public int Type { get; set; }

        public long UniqueTarget { get; set; }

        public bool IsHandled { get; set; }

        public TMessage TypeCheck<TMessage>()
            where TMessage : class, IMessage
        {
            var msg = this as TMessage;
            if (msg == null)
            {
                throw new ArgumentException(string.Format(
                    "Message has actual type \"{0}\" but it's expected to have type \"{1}\".",
                    this.GetType(),
                    typeof(TMessage)));
            }

            return msg;
        }

        #region IPoolItem implementation

        void IPoolItem.Acquire()
        {
            Type = type;
        }

        void IPoolItem.Release()
        {
            Data = default(T);
            Time = null;
            Type = MessageType.Unknown;
            UniqueTarget = -1;
        }

        #endregion

        private int type;
    }
}
