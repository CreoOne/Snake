using System;

namespace Snake.Engine
{
    public class FrameCounter
    {
        private DateTime PreviousTime = DateTime.UtcNow;
        public double FPS { get; private set; }
        public double SmoothFPS { get; private set; }
        public double Delta { get; private set; }

        public void Tick()
        {
            Delta = (DateTime.UtcNow - PreviousTime).TotalMilliseconds / 1000;
            PreviousTime = DateTime.UtcNow;
            FPS = 1 / Delta;
            SmoothFPS = RollingAverage.Average(SmoothFPS, FPS, FPS);
        }
    }
}
