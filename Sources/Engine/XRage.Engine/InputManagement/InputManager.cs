using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Configuration;
using System.Xml.Linq;

namespace AISTek.XRage.InputManagement
{
    public class InputManager : XComponent
    {
        public InputManager(XGame game)
            : base(game)
        {
            InputState = new InputState(this);
            Actions = new Dictionary<string, NamedAction>();
        }

        public InputState InputState { get; private set; }

        public MouseInputState Mouse { get { return InputState.Mouse; } }

        public IDictionary<string, NamedAction> Actions { get; private set; }

        public bool IsMouseLocked { get; set; }

        public float MouseSensitivity
        {
            get { return mouseSensitivity; }
            set { mouseSensitivity = MathHelper.Clamp(value, 0.0f, 1.0f); }
        }

        public NamedAction GetAction(string name)
        {
            if (!Actions.ContainsKey(name))
                return null;

            return Actions[name];
        }

        public void AddAction(string name, NamedAction input)
        {
            if (Actions.ContainsKey(name))
                throw new ArgumentException("Input with specified name already exists.");

            Actions.Add(name, input);
        }

        public void Update(GameTime gameTime)
        {
            InputState.Update(gameTime);

            foreach (var action in Actions.Values)
            {
                action.Update(gameTime, InputState);
            }
        }

        public void LoadInputConfiguration(ConfigurationStorage storage)
        {
            var factory = new InputBindingFactory();
            var root = storage.LoadInputConfiguration();

            Actions = (from inputElement in root.Descendants("actions")
                                               .First()
                                               .Descendants("action")
                      let name = inputElement.Attribute("name").Value
                      let bindings = from bindingElement in inputElement.Descendants("bindings")
                                                                        .First()
                                                                        .Descendants()
                                     select factory.CreateBinding(bindingElement)
                      select new NamedAction(name, bindings))
                      .ToDictionary(input => input.Name, input => input);

            MouseSensitivity = Game.Settings.GameSettings.MouseSensitivity;
        }

        public void SaveInputConfiguration(ConfigurationStorage storage)
        {
            using (var transaction = storage.SaveInputConfiguration())
            {
                var writer = new InputBindingWriter();

                transaction.Root.Add(new XElement("actions",
                    (from input in Actions.Values
                     let bindings = from binding in input.Bindings
                                    select writer.WriteBinding(binding)
                     select new XElement("action",
                         new XAttribute("name", input.Name),
                         new XElement("bindings", bindings)))));
            }

            Game.Settings.GameSettings.MouseSensitivity = MouseSensitivity;
        }

        private float mouseSensitivity = 0.5f;
    }
}
