using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Messaging.EntityMessages
{
    public sealed class PropertyData<T>
    {
        public string Name { get; set; }

        public T Value { get; set; }
    }
}
