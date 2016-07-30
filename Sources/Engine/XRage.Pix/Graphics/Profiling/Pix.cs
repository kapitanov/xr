using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace AISTek.XRage.Graphics.Profiling
{
    /// <summary>
    /// Contains static methods for creating events in markers
    /// used during PIX profiling.
    /// </summary>
    public static class Pix
    {
        static Pix()
        {
            try
            {
                if (IsPixAttached)
                {
                    driver = new PixEnabledDriver();
                }
                else
                {
                    driver = new PixEnabledDriver();
                }
            }
            catch
            {
                driver = new PixDisabledDriver();
            }
        }

        /// <summary>
        /// Determines whether PIX is attached to the running process
        /// </summary>
        /// <returns>true if PIX is attached, false otherwise</returns>
        public static bool IsPixAttached
        {
            get { return (NativeMethods.D3DPERF_GetStatus() > 0); }
        }

        /// <summary>
        /// Calling this method will prevent PIX from being able
        /// to attach to this process.
        /// </summary>
        public static void DisablePixProfiling()
        {
            NativeMethods.D3DPERF_SetOptions(1);
        }

        /// <summary>
        /// Marks the beginning of a PIX event.  Events are used to group
        /// together a series of related API calls, for example a series of
        /// commands needed to draw a single model.  Events can be nested.
        /// </summary>
        /// <param name="eventName">The name of the event</param>
        public static void BeginEvent(string eventName)
        {
            driver.BeginEvent(eventName);
            eventCount++;
        }

        /// <summary>
        /// Marks the end of a PIX event.
        /// </summary>
        public static void EndEvent()
        {
            // Make sure we haven't called EndEvent more times
            // than we've called BeginEvent
            //Contract.Assert(eventCount >= 1);

            driver.EndEvent();
            eventCount--;
        }

        /// <summary>
        /// Adds a marker in PIX.  A marker is used to indicate that single
        /// instantaneous event has occurred.
        /// </summary>
        /// <param name="markerName"></param>
        public static void SetMarker(string markerName)
        {
            driver.SetMarker(markerName);
        }

        #region Private fields

        private static IPixDriver driver;

        /// <summary>
        /// Keeps track of our BeginEvent/EndEvent pairs
        /// </summary>
        private static int eventCount = 0;

        #endregion
    }
}
