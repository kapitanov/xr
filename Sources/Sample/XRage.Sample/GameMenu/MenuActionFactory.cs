using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Sample.GameMenu
{
    public class MenuActionFactory : IMenuActionFactory
    {
        public MenuActionFactory()
        {
            RegisterConstructor<LoadSceneMenuActionConstructor>();
            RegisterConstructor<ExitGameMenuActionConstructor>();
        }

        public void RegisterConstructor<T>()
            where T: IBaseMenuActionConstructor, new()
        {
            RegisterConstructor(new T());
        }

        public void RegisterConstructor(IBaseMenuActionConstructor constructor)
        {
            if (constructors.ContainsKey(constructor.KnownType))
            {
                throw new ArgumentException(string.Format("Constructor for \"{0}\" is already defined.", constructor.KnownType));
            }

            constructors.Add(constructor.KnownType, constructor);
        }

        public BaseMenuAction CreateMenuAction(XElement element, XGame game, GameMenuItem menuItem)
        {
            var type = element.Name.ToString();

            if (!constructors.ContainsKey(type))
            {
                throw new ArgumentException(string.Format("Type \"{0}\" is unknown.", type));
            }

            return constructors[type].CreateMenuAction(element, game, menuItem);
        }

        private IDictionary<string, IBaseMenuActionConstructor> constructors = new Dictionary<string, IBaseMenuActionConstructor>();
    }
}
