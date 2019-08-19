using EntityComponentFramework.Components;
using System;
using System.Numerics;

namespace Snake.Components
{
    public class Position : IComponent
    {
        private static readonly Random RNG = new Random(DateTime.UtcNow.Millisecond);
        public Vector2 Coordinates { get; set; }

        public void Random()
        {
            Coordinates = new Vector2(RNG.Next(GameConfig.Width), RNG.Next(GameConfig.Height));
        }

        public void Center()
        {
            Coordinates = new Vector2(GameConfig.Width / 2, GameConfig.Height / 2);
        }
    }
}
