using EntityComponentFramework;
using EntityComponentFramework.Components;
using Snake.Components;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Snake.Incubators
{
    public class AppleIncubator : IIncubator
    {
        public Entity Create()
        {
            List<IComponent> apple = new List<IComponent>();
            apple.Add(new Apple());
            Position position = new Position(TimeSpan.FromMilliseconds(100), Morph);
            position.TeleportRandom();
            apple.Add(position);
            return new Entity(apple);
        }

        private Vector2 Morph(double position, Vector2 origin, Vector2 target)
        {
            return origin + (target - origin) * (float)Math.Sin(position * (Math.PI / 2));
        }
    }
}
