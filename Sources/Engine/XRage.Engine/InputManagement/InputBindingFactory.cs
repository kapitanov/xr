using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AISTek.XRage.Configuration;

namespace AISTek.XRage.InputManagement
{
    public class InputBindingFactory
    {
        public InputBindingFactory()
        {
            RegisterConstructor<KeyboardInputBindingConstructor>();
        }

        public void RegisterConstructor<T>()
            where T : IInputBindingConstructor, new()
        {
            RegisterConstructor(new T());
        }

        public void RegisterConstructor(IInputBindingConstructor constructor)
        {
            if (constructors.ContainsKey(constructor.KnownType))
                throw new ArgumentException(
                    string.Format("Input binding constructor for type \"{0}\" is already defined.", constructor.KnownType));

            constructors.Add(constructor.KnownType, constructor);
        }

        public InputBinding CreateBinding(XElement bindingElement)
        {
            var name = bindingElement.Name.ToString();
            if (!constructors.ContainsKey(name))
                throw new ArgumentException(string.Format("Input binding of type \"{0}\" is unknown.", name));

            return constructors[name].CreateInputBinding(bindingElement);
        }

        private IDictionary<string, IInputBindingConstructor> constructors = new Dictionary<string, IInputBindingConstructor>();
    }
}
