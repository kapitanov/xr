using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AISTek.XRage
{
    public class FpsCounter : XComponent
    {
        public FpsCounter(XGame game)
            : base(game)
        {
            timer = Stopwatch.StartNew();
        }

        public int FramesPerSecond { get; private set; }

        public void OnDrawFrame()
        {
            frameCount++;

            if (timer.ElapsedMilliseconds > 750.0)
            {
                FramesPerSecond = (int)(frameCount * 1000 / timer.ElapsedMilliseconds);

                frameCount = 0;

                timer.Reset();
                timer.Start();
            }
        }

        private Stopwatch timer;
        private int frameCount;
    }
}
