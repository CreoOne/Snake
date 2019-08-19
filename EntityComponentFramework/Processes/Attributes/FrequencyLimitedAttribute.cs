using System;

namespace EntityComponentFramework.Processes.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class FrequencyLimitedAttribute : Attribute
    {
        public int Interval { get; }
        public bool InitialTrigger { get; }
        
        public FrequencyLimitedAttribute(int interval, bool initialTrigger = false)
        {
            Interval = interval;
            InitialTrigger = initialTrigger;
        }
    }
}
