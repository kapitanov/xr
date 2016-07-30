using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.InputManagement
{
    public class NamedAction
    {
        public NamedAction(string name)
        {
            Name = name;
            IsOn = false;
            Bindings = new List<InputBinding>();
        }

        public NamedAction(string name, IEnumerable<InputBinding> bindings)
            : this(name)
        {
            Bindings = bindings.ToList();
        }

        public string Name { get; set; }

        internal IList<InputBinding> Bindings { get; private set; }

        public bool IsOn { get; private set; }

        public event EventHandler OnFire;

        internal void Update(GameTime gameTime, InputState inputState)
        {
            IsOn = Bindings.Any(binding => binding.EvaluateBinding(inputState));

            if (IsOn)
            {
                InvokeOnFire();
            }
        }

        private void InvokeOnFire()
        {
            var handler = OnFire;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
