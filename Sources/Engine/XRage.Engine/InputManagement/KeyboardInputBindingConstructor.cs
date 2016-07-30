using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace AISTek.XRage.InputManagement
{
    public class KeyboardInputBindingConstructor : IInputBindingConstructor
    {
        public string KnownType { get { return "keyboard"; } }
        
        public InputBinding CreateInputBinding(XElement element)
        {
            Keys key;

            if (element.Attribute("key") == null)
            {
                throw new ArgumentException(string.Format(
                    "Keyboard action binding must have a \"key\" attribute. ({0}, {1})", 
                    (element as IXmlLineInfo).LineNumber,
                    (element as IXmlLineInfo).LinePosition));
            }

            var value = element.Attribute("key").Value;

            if(!Enum.TryParse<Keys>(value, out key))
            {
                throw new ArgumentException(string.Format("Value \"{0}\" is not valid value of {1}", value, typeof(Keys)));
            }

            var actionType = ActionType.Press;
            if (element.Attribute("type") != null)
            {
                value = element.Attribute("type").Value;
                if (!Enum.TryParse<ActionType>(value, out actionType))
                {
                    throw new ArgumentException(string.Format("Value \"{0}\" is not valid value of {1}", value, typeof(ActionType)));
                }
            }

            return new KeyboardInputBinding(key, actionType);
        }
    }
}
