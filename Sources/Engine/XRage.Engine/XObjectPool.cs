using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage
{
    /// <summary>
    /// Provides managed resources which are not disposed in order to prevent GC
    /// </summary>
    public static class XObjectPool
    {
        /// <summary>
        /// Acquires a new item from the pool
        /// </summary>
        /// <typeparam name="T">
        /// Type of the item to retrieve
        /// </typeparam>
        /// <returns>
        /// An instance of <typeparamref name="T"/>, which might be already instantiated
        /// </returns>
        public static T Acquire<T>()
            where T : class, IPoolItem, new()
        {
            var type = typeof(T);

            lock (acquireLock)
            {
                if (free.ContainsKey(type) == false)
                {
                    free.Add(type, new List<IPoolItem>(PoolIncrement));
                }

                if (locked.ContainsKey(type) == false)
                {
                    locked.Add(type, new List<IPoolItem>(PoolIncrement));
                }

                List<IPoolItem> items = free[type];

                if (items.Count == 0)
                {
                    for (int i = 0; i < PoolAllocationSize; ++i)
                    {
                        items.Add(new T());
                    }
                }

                var item = items[0] as T;

                // TODO: Find out why this is happening, it shouldn't be
                if (item == null)
                {
                    throw new Exception("Bad data in items container");
                }

                items.RemoveAt(0);

                locked[type].Add(item);

                item.Acquire();
                item.IsHandled = false;
                return item;
            }
        }

        /// <summary>
        /// Releases a item and puts it back in the available stack
        /// </summary>
        /// <typeparam name="T">
        /// Type of the item to release
        /// </typeparam>
        /// <param name="item">
        /// The instance of <typeparamref name="T"/> which should be released
        /// </param>
        /// <exception cref="ArgumentException">
        /// Is thrown if the <paramref name="item"/> was not created using <see cref="ObjectPool.Aquire"/>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <paramref name="item"/> is null
        /// </exception>
        public static void Release<T>(T item)
            where T : class, IPoolItem, new()
        {
            Release(item, typeof(T));
        }

        public static void Release(IPoolItem item, Type type)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (locked[type].Contains(item) == false)
            {
                // TODO: Find out why this is even necessary
                if (locked[type].Count == 0)
                {
                    locked.Remove(type);
                }

                throw new ArgumentException("Release item has not been acquired");
            }
            else
            {
                locked[type].Remove(item);
            }

            item.Release();

            free[type].Add(item);
        }

        /// <summary>
        /// Releases all items freeing all queues
        /// </summary>
        public static void ReleaseAll()
        {
            // Release and clear all locked items first
            foreach (var lockedItem in locked)
            {
                foreach (var element in lockedItem.Value)
                {
                    element.Release();
                }
                lockedItem.Value.Clear();
            }

            // Then release and clear all free items 
            foreach (var freeItem in free)
            {
                freeItem.Value.Clear();
            }
        }

        #region Private fields

        private static readonly Dictionary<Type, List<IPoolItem>> free =
            new Dictionary<Type, List<IPoolItem>>(PoolIncrement);

        private static readonly Dictionary<Type, List<IPoolItem>> locked =
            new Dictionary<Type, List<IPoolItem>>(PoolIncrement);

        private static readonly object acquireLock = new object();

        private const int PoolAllocationSize = 5;

        private const int PoolIncrement = 20;

        #endregion
    }

}
