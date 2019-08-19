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

        public Processor(IEnumerable<IProcess> processes, IEnumerable<Entity> entities)
        {
            Processes = new HashSet<IProcess>(processes);
            Entities = new HashSet<Entity>(entities);
            InstanceFrequencyLimiters = new Dictionary<object, FrequencyLimiter>();
        }

        public void Advance()
        {
            foreach (IProcess process in Processes)
            {
                Type processType = process.GetType();

                if (!TriggerInstance(processType, process))
                    continue;

                ExecuteAllMethods<BeforeAllAttribute>(process, processType);
                ExecuteMethods(process, processType);
                ExecuteAllMethods<AfterAllAttribute>(process, processType);
            }

        }

        private void ExecuteMethods(IProcess process, Type processType)
        {
            IEnumerable<MethodInfo> executeMethods = processType
                .GetMethods(Flags)
                .Where(m => m.ReturnType.Equals(VoidType))
                .Where(m => m.GetParameters().All(p => p.ParameterType.Equals(EntityType)));

            foreach (MethodInfo method in executeMethods)
            {
                ParameterInfo[] parameters = method.GetParameters();

                switch (parameters.Length)
                {
                    case 1:

                        foreach (Entity entity in FilterEntities(parameters[0], Entities))
                            TriggerMethod(method, process, new[] { entity });
                        break;

                    case 2:
                        foreach (Entity primary in FilterEntities(parameters[0], Entities))
                            foreach (Entity secondary in FilterEntities(parameters[1], Entities))
                                TriggerMethod(method, process, new[] { primary, secondary });
                        break;
                }
            }
        }

        private static IEnumerable<Entity> FilterEntities(ParameterInfo parameter, IEnumerable<Entity> entites)
        {
            HasAttribute hasAttribute = parameter.GetCustomAttribute<HasAttribute>();

            if (hasAttribute == null)
                return entites;

            return entites
                .Where(e => hasAttribute.Components.All(ac => e.Any(ec => ac.Equals(ec.GetType()))));
        }

        private void ExecuteAllMethods<TAttribute>(IProcess process, Type processType) where TAttribute : Attribute
        {
            IEnumerable<MethodInfo> allMethods = processType
                .GetMethods(Flags)
                .Where(m => m.ReturnType.Equals(VoidType))
                .Where(m => m.GetCustomAttribute<TAttribute>() != null)
                .Where(m => m.GetParameters().Count() == 0);

            foreach (MethodInfo method in allMethods)
                TriggerMethod(method, process, new object[] { });
        }
        
        private void TriggerMethod(MethodInfo method, object owner, object[] parameters)
        {
            if (TriggerInstance(method, owner))
                method.Invoke(owner, parameters);
        }
        
        private bool TriggerInstance(MemberInfo instanceType, object owner)
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
