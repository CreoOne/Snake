using System;

namespace EntityComponentFramework.Processes.Attributes
{
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class HasAttribute : Attribute
    {
        public Type[] Components { get; }

        public HasAttribute(params Type[] components)
        {
            Components = components;
        }
    }
}
