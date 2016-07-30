using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage
{
    public interface IInterfaceManager
    {
        XInterface QueryInterface(InterfaceType xInterface);

        T QueryInterface<T>()
            where T : XInterface;

        void AddInterface(XInterface xInterface);
    }
}
