using EntityComponentFramework.Components;
using Snake.Engine;
using System;
using System.Numerics;

namespace Snake.Components
{
    public class Position : IComponent
    {
        private static readonly Random RNG = new Random(DateTime.UtcNow.Millisecond);
        public Tween<Vector2> Coordinates { get; set; }

        public Position(TimeSpan tweenSpan, Func<double, Vector2, Vector2, Vector2> morph)
        {
            Coordinates = new Tween<Vector2>(tweenSpan, morph, Vector2.Zero);
        }

        public void TeleportRandom()
        {
            SetCoordinates(new Vector2(RNG.Next(GameConfig.BoardWidth), RNG.Next(GameConfig.BoardHeight)));
        }

        public void MoveRandom()
        {
            Coordinates.Target = new Vector2(RNG.Next(GameConfig.BoardWidth), RNG.Next(GameConfig.BoardHeight));
        }

        public void TeleportCenter()
        {
            SetCoordinates(new Vector2(GameConfig.BoardWidth / 2, GameConfig.BoardHeight / 2));
        }

        private void SetCoordinates(Vector2 vector)
        {
            Coordinates = new Tween<Vector2>(Coordinates.Span, Coordinates.Morph, vector);
        }
    }
}
