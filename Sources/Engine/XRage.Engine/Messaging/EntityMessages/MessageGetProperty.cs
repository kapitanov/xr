using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public abstract class MessageGetProperty<T> : Message<PropertyData<T>>
    {
        protected MessageGetProperty(int type)
            : base(type)
        { }

        public string PropertyName
        {
            get { return Data.Name; }
            set { Data.Name = value; }
        }

        public T PropertyValue
        {
            get { return Data.Value; }
            set { Data.Value = value; }
        }
    }
}
