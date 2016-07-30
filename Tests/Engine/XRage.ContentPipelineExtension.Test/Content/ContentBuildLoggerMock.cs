using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Diagnostics;

namespace AISTek.XRage.Content
{
    internal class ContentBuildLoggerMock : ContentBuildLogger
    {
        public override void LogImportantMessage(string message, params object[] messageArgs)
        {
            Debug.Print(message, messageArgs);
        }

        public override void LogMessage(string message, params object[] messageArgs)
        {
            Debug.Print(message, messageArgs);
        }

        public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
        {
            Debug.Print(message, messageArgs);
        }
    }
}
