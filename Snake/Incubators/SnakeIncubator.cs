using EntityComponentFramework;
using EntityComponentFramework.Components;
using Snake.Components;
using System.Collections.Generic;

namespace Snake.Incubators
{
    public class SnakeInkubator : IIncubator
    {
        public Entity Create()
        {
            List<IComponent> tail = new List<IComponent>();
            tail.Add(new Tail());
            Position position = new Position();
            position.Center();
            tail.Add(position);
            return new Entity(tail);
        }
    }
}
