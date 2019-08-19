using System;

namespace EntityComponentFramework
{
    public class FrequencyLimiter
    {
        private DateTime LastUpdate { get; set; }
        private int Interval { get; }

        public FrequencyLimiter(int intervalInMiliseconds, bool initialTrigger)
        {
            Interval = intervalInMiliseconds;
            LastUpdate = initialTrigger ? DateTime.UtcNow : DateTime.UtcNow + TimeSpan.FromMilliseconds(Interval);
        }

        public bool Trigger()
        {
            if (DateTime.UtcNow < LastUpdate)
                return false;

            LastUpdate += TimeSpan.FromMilliseconds(Interval);
            return true;
        }
    }
}
