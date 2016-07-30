using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.InputManagement
{
    public class KeyboardInputBindingWriter : IInputBindingWriter
    {
        public Type KnownType{get{return typeof(KeyboardInputBinding);}}

        public XElement WriteBinding(InputBinding inputBinding)
        {
            var binding = inputBinding as KeyboardInputBinding;
            return new XElement("keyboard",
                new XAttribute("key", binding.Key.ToString()),
                new XAttribute("type", binding.ActionType.ToString()));
        }
    }
}
