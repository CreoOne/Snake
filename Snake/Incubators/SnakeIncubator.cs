using EntityComponentFramework;
using EntityComponentFramework.Components;
using Snake.Components;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Snake.Incubators
{
    public class SnakeIncubator : IIncubator
    {
        public Entity Create()
        {
            TimeSpan tweenSpan = TimeSpan.FromMilliseconds(GameConfig.SnakeMovementInterval * 0.6);
            List<IComponent> tail = new List<IComponent>();
            tail.Add(new Tail(tweenSpan, Morph));
            Position position = new Position(tweenSpan, Morph); 
            position.TeleportCenter();
            tail.Add(position);
            return new Entity(tail);
        }

        private Vector2 Morph(double position, Vector2 origin, Vector2 target)
        {
            return origin + (target - origin) * (float)Math.Sin(position * (Math.PI / 2));
        }
    }
}
