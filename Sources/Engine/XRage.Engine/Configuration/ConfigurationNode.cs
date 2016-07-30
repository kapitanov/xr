using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AISTek.XRage.Configuration
{
    public class ConfigurationNode : IEnumerable<ConfigurationNode>
    {
        public ConfigurationNode(string name)
        {
            Name = name;
            Properties = new Dictionary<string, string>();
            ChildNodes = new List<ConfigurationNode>();
        }

        public ConfigurationNode(XElement element)
        {
            Name = element.Name.ToString();
            Properties = element.Attributes()
                                .ToDictionary(attribute => attribute.Name.ToString(), attribute => attribute.Value);
            ChildNodes = element.Descendants()
                                .Select(node => new ConfigurationNode(node))
                                .ToList();
        }

        public string Name { get; private set; }

        public IDictionary<string, string> Properties { get; private set; }

        public IList<ConfigurationNode> ChildNodes { get; private set; }

        public ConfigurationNode Add(string property, string value)
        {
            if (Properties.ContainsKey(property))
            {
                Properties[property] = value;
            }
            else
            {
                Properties.Add(property, value);
            }

            return this;
        }

        public ConfigurationNode Add(KeyValuePair<string, string> property)
        {
            if (Properties.ContainsKey(property.Key))
            {
                Properties[property.Key] = property.Value;
            }
            else
            {
                Properties.Add(property);
            }

            return this;
        }

        public ConfigurationNode Add(ConfigurationNode node)
        {
            if (ChildNodes.Contains(node))
                throw new InvalidOperationException("Node already exists");

            ChildNodes.Add(node);
            return this;
        }

        public IEnumerator<ConfigurationNode> GetEnumerator()
        {
            return ChildNodes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string GetProperty(string property)
        {
            if (Properties.ContainsKey(property))
            {
                return Properties[property];
            }

            throw new KeyNotFoundException();
        }

        public string this[string property]
        {
            get { return GetProperty(property); }
            set { Add(property, value); }
        }

        public ConfigurationNode GetNode(string name)
        {
            return ChildNodes.FirstOrDefault(node => node.Name == name);
        }

        public XElement ToXml()
        {
            var element = new XElement(Name);

            element.Add(Properties.Select(property => new XAttribute(property.Key, property.Value)).ToArray());
            element.Add(ChildNodes.Select(node => node.ToXml()).ToArray());

            return element;
        }
    }
}
