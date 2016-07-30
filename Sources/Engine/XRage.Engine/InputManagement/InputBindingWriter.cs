using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.InputManagement
{
    public class InputBindingWriter
    {
        public InputBindingWriter()
        {
            RegisterWriter<KeyboardInputBindingWriter>();
        }

        public void RegisterWriter<T>()
            where T : IInputBindingWriter, new()
        {
            RegisterWriter(new T());
        }

        public void RegisterWriter(IInputBindingWriter writer)
        {
            if (writers.ContainsKey(writer.KnownType))
                throw new ArgumentException(
                    string.Format("Input binding writer for type \"{0}\" is already defined.", writer.KnownType));

            writers.Add(writer.KnownType, writer);
        }          

        public XElement WriteBinding(InputBinding binding)
        {
            var type = binding.GetType();
            if (!writers.ContainsKey(type))
                throw new ArgumentException(string.Format("Unable to write binding of type {0}: type is unknown.", type));

            return writers[type].WriteBinding(binding);
        }

        private IDictionary<Type, IInputBindingWriter> writers = new Dictionary<Type, IInputBindingWriter>();
    }
}
