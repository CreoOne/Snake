using EntityComponentFramework.Processes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using EntityComponentFramework.Processes.Attributes;

namespace EntityComponentFramework
{
    public class Processor
    {
        private static readonly Type VoidType = typeof(void);
        private static readonly Type EntityType = typeof(Entity);
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
        private HashSet<IProcess> Processes { get; }
        private HashSet<Entity> Entities { get; }
        private Dictionary<object, FrequencyLimiter> InstanceFrequencyLimiters { get; }
        private Dictionary<MethodInfo, FrequencyLimiter> MethodFrequencyLimiters { get; }

        public Processor(IEnumerable<IProcess> processes, IEnumerable<Entity> entities)
        {
            Processes = new HashSet<IProcess>(processes);
            Entities = new HashSet<Entity>(entities);
            InstanceFrequencyLimiters = new Dictionary<object, FrequencyLimiter>();
            MethodFrequencyLimiters = new Dictionary<MethodInfo, FrequencyLimiter>();
        }

        public void Advance()
        {
            foreach (IProcess process in Processes)
            {
                Type processType = process.GetType();

                if (!Trigger(processType, process))
                    continue;

                BeforeAllMethods(process, processType);
                ExecuteMethods(process, processType);
                AfterAllMethods(process, processType);
            }

        }

        private void AfterAllMethods(IProcess process, Type processType)
        {
            IEnumerable<MethodInfo> afterAllMethods = processType
                .GetMethods(Flags)
                .Where(m => m.ReturnType.Equals(VoidType))
                .Where(m => m.GetCustomAttribute<AfterAllAttribute>() != null)
                .Where(m => m.GetParameters().Count() == 0);

            foreach (MethodInfo method in afterAllMethods)
                Trigger(process, method, new object[] { });
        }

        private void ExecuteMethods(IProcess process, Type processType)
        {
            IEnumerable<MethodInfo> executeMethods = processType
                .GetMethods(Flags)
                .Where(m => m.ReturnType.Equals(VoidType))
                .Where(m => m.GetParameters().All(p => p.ParameterType.Equals(EntityType)));

            foreach (MethodInfo method in executeMethods)
            {
                switch (method.GetParameters().Length)
                {
                    case 1:
                        foreach (Entity entity in Entities)
                            Trigger(process, method, new[] { entity });
                        break;

                    case 2:
                        foreach (Entity primary in Entities)
                            foreach (Entity secondary in Entities)
                                Trigger(process, method, new[] { primary, secondary });
                        break;
                }
            }
        }

        private void BeforeAllMethods(IProcess process, Type processType)
        {
            IEnumerable<MethodInfo> beforeAllMethods = processType
                .GetMethods(Flags)
                .Where(m => m.ReturnType.Equals(VoidType))
                .Where(m => m.GetCustomAttribute<BeforeAllAttribute>() != null)
                .Where(m => m.GetParameters().Count() == 0);

            foreach (MethodInfo method in beforeAllMethods)
                Trigger(process, method, new object[] { });
        }

        private void Trigger(IProcess owner, MethodInfo method, params object[] parameters)
        {
            FrequencyLimitedAttribute frequencyLimitedAttribute = method.GetCustomAttribute<FrequencyLimitedAttribute>();

            if (frequencyLimitedAttribute == null)
            {
                method.Invoke(owner, parameters);
                return;
            }

            if (!MethodFrequencyLimiters.ContainsKey(method))
                MethodFrequencyLimiters.Add(method, new FrequencyLimiter(frequencyLimitedAttribute.Interval, frequencyLimitedAttribute.InitialTrigger));

            if (MethodFrequencyLimiters[method].Trigger())
                method.Invoke(owner, parameters);
        }
        
        private bool Trigger(Type instanceType, object owner)
        {
            FrequencyLimitedAttribute frequencyLimitedAttribute = instanceType.GetCustomAttribute<FrequencyLimitedAttribute>();

            if (frequencyLimitedAttribute == null)
                return true;

            if (!InstanceFrequencyLimiters.ContainsKey(owner))
                InstanceFrequencyLimiters.Add(owner, new FrequencyLimiter(frequencyLimitedAttribute.Interval, frequencyLimitedAttribute.InitialTrigger));

            return InstanceFrequencyLimiters[owner].Trigger();
        }
    }
}
