using EntityComponentFramework;
using EntityComponentFramework.Components;
using Snake.Components;
using System.Collections.Generic;

namespace Snake.Incubators
{
    public class AppleIncubator : IIncubator
    {
        public Entity Create()
        {
            List<IComponent> apple = new List<IComponent>();
            apple.Add(new Apple());
            Position position = new Position();
            position.Random();
            apple.Add(position);
            return new Entity(apple);
        }
    }
}
