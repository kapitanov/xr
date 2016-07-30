using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage
{
    /// <summary>
    /// Interface for poolable item
    /// </summary>
    public interface IPoolItem
    {
        /// <summary>
        /// This releases a item freeing up all allocated resources
        /// </summary>
        void Release();

        /// <summary>
        /// This reassings the item, this method should be used to reinitialize the item
        /// </summary>
        void Acquire();

        bool IsHandled { get; set; }
    }
}
