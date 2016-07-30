using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public abstract class MessageModifyProperty<T> : Message<PropertyData<T>>
    {
        protected MessageModifyProperty(int type)
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
