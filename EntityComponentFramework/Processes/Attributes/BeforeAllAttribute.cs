using System;

namespace EntityComponentFramework.Processes.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class BeforeAllAttribute : Attribute
    {

    }
}
