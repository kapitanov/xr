using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.VmfParsing
{
    internal class VmfStreamEnumerator : IEnumerator<char>
    {
        public VmfStreamEnumerator(IEnumerable<char> stream)
        {
            enumerator = stream.GetEnumerator();
        }

        public int Line { get; private set; }

        public int Column { get; private set; }

        public char Current { get { return enumerator.Current; } }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        object System.Collections.IEnumerator.Current { get { return Current; } }

        public bool MoveNext()
        {
            if (enumerator.MoveNext())
            {
                if (enumerator.Current != '\n')
                {
                    Column++;
                }
                else
                {
                    Column = 0;
                    Line++;
                }

                return true;
            }

            return false;
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        private readonly IEnumerator<char> enumerator;
    }
}
