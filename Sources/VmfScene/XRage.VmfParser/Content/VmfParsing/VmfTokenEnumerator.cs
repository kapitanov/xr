using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.VmfParsing
{
    internal class VmfTokenEnumerator : IEnumerator<VmfToken>
    {
        public VmfTokenEnumerator(IEnumerable<VmfToken> tokens)
        {
            enumerator = tokens.GetEnumerator();
        }

        public VmfToken Current { get { return enumerator.Current; } }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        object System.Collections.IEnumerator.Current { get { return Current; } }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        private readonly IEnumerator<VmfToken> enumerator;
    }
}
