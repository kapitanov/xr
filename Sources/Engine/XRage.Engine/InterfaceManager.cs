using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics.Contracts;

namespace AISTek.XRage
{
    internal class InterfaceManager : IInterfaceManager
    {
        public InterfaceManager()
        {
            InterfacesByCode = new Dictionary<InterfaceType, XInterface>();
            InterfacesByType = new Dictionary<Type, XInterface>();
        }

        public IDictionary<InterfaceType, XInterface> InterfacesByCode { get; private set; }

        public IDictionary<Type, XInterface> InterfacesByType { get; private set; }

        public XInterface QueryInterface(InterfaceType xInterface)
        {
            if (InterfacesByCode.ContainsKey(xInterface))
                return InterfacesByCode[xInterface];

            throw new KeyNotFoundException(string.Format("Interface <{0}> is not defined", xInterface));
        }

        public T QueryInterface<T>()
            where T : XInterface
        {
            var type = typeof(T);
            if (InterfacesByType.ContainsKey(type))
                return (T)InterfacesByType[type];

            throw new KeyNotFoundException(string.Format("Interface {0} is not defined", type));
        }

        public void Update(GameTime gameTime)
        {
            foreach (var xInterface in InterfacesByCode.Values)
            {
                xInterface.Update(gameTime);
            }
        }

        public void Shutdown()
        {
            foreach (var xInterface in InterfacesByCode.Values)
            {
                xInterface.Shutdown();
            }
        }

        public void AddInterface(XInterface xInterface)
        {
            var type = xInterface.GetType();
            var code = xInterface.InterfaceType;

            if (InterfacesByCode.ContainsKey(code))
            {
                throw new ArgumentException(string.Format("Interface <{0}> is already defined", code));
            }

            if (InterfacesByType.ContainsKey(type))
            {
                throw new ArgumentException(string.Format("Interface {0} is already defined", type));
            }

            InterfacesByCode.Add(code, xInterface);
            InterfacesByType.Add(type, xInterface);
        }
    }
}
