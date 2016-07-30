using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Messaging
{
    /// <summary>
    /// Interface for engine messages
    /// </summary>
    public interface IMessage : IPoolItem
    {
        /// <summary>
        /// The type of the message
        /// </summary>
        int Type { get; }

        /// <summary>
        /// Unique target's Id of current message
        /// </summary>
        long UniqueTarget { get; }

        /// <summary>
        /// Perfrorms type-safe up-convertion of message. 
        /// </summary>
        /// <typeparam name="T">
        /// A data class to convert.
        /// </typeparam>
        /// <returns>
        /// An instance of <typeparamref name="T" /> class.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if actual type of message is not type <typeparamref name="T" />
        /// </exception>
        TMessage TypeCheck<TMessage>()
            where TMessage : class, IMessage;
    }
}
