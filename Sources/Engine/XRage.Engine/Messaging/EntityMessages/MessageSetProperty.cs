using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public abstract class MessageSetProperty<T> : Message<PropertyData<T>>
    {
        protected MessageSetProperty(int type)
            : base(type)
        {
            Data = new PropertyData<T>();
        }

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
