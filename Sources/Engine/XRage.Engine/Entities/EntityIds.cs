using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Entities
{
    internal static class EntityIds
    {
        public static long RequestUniqueId()
        {
            return ++LastUsedId;
        }

        private static long LastUsedId = 0;
    }
}
