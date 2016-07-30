using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.VmfParsing
{
    public class VmfNodeCollection : ICollection<VmfNode>
    {
        public VmfNodeCollection()
        {
            nodes = new List<VmfNode>();
        }

        public VmfNodeCollection(IEnumerable<VmfNode> nodeList)
        {
            nodes = new List<VmfNode>(nodeList);
        }

        public void Add(VmfNode node)
        {
            nodes.Add(node);
        }

        public void Clear()
        {
            nodes.Clear();
        }

        public bool Contains(VmfNode node)
        {
            return nodes.Contains(node);
        }

        public void CopyTo(VmfNode[] array, int arrayIndex)
        {
            nodes.CopyTo(array, arrayIndex);
        }

        public int Count { get { return nodes.Count; } }

        public bool IsReadOnly { get { return false; } }

        public bool Remove(VmfNode node)
        {
            return nodes.Remove(node);
        }

        public IEnumerator<VmfNode> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<VmfClassNode> GetListNodes()
        {
            return nodes.OfType<VmfClassNode>();
        }

        public IEnumerable<VmfPropertyNode> GetAttributeNodes()
        {
            return nodes.OfType<VmfPropertyNode>();
        }

        private List<VmfNode> nodes;
    }
}
