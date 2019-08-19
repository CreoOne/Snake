using EntityComponentFramework.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentFramework
{
    public class Entity : IEnumerable<IComponent>
    {
        private List<IComponent> Components { get; }

        public Entity(IEnumerable<IComponent> components)
        {
            Components = new List<IComponent>(components);
        }

        public bool Has<TComponent0>() where TComponent0 : IComponent
        {
            return Components.Any(c => c is TComponent0);
        }

        public bool Has<TComponent0, TComponent1>() where TComponent0 : IComponent where TComponent1 : IComponent
        {
            return Components.Any(c => c is TComponent0) && Components.Any(c => c is TComponent1);
        }

        public TComponent GetFirst<TComponent>() where TComponent : IComponent
        {
            return (TComponent)Components.FirstOrDefault(c => c is TComponent);
        }

        public IEnumerable<TComponent> GetAll<TComponent>() where TComponent : IComponent
        {
            return Components.OfType<TComponent>();
        }

        public IEnumerator<IComponent> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
