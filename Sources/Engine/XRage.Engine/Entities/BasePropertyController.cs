using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Entities
{
    public abstract class BasePropertyController : BaseController
    {
        protected BasePropertyController(XGame game, string name, string controlledEntityName, string propertyName)
            : base(game, name, controlledEntityName)
        {
            ControlledPropertyName = propertyName;
        }

        public string ControlledPropertyName { get; private set; }
    }
}
