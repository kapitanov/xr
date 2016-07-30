using System;

namespace AISTek.XRage.Sample
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new SampleGame())
            {
                game.Run();
            }
        }
    }
#endif
}

