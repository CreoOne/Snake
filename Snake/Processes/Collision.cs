using EntityComponentFramework;
using EntityComponentFramework.Processes;
using Snake.Components;
using Snake.MovementDirection;
using System.Drawing;
using System.Linq;

namespace Snake.Processes
{
    public class Collision : IProcess
    {
        public void Execute(Entity entity)
        {
            if (!entity.Has<Tail, Position>())
                return;

            Position position = entity.GetFirst<Position>();
            Tail tail = entity.GetFirst<Tail>();

            if (tail != null && (tail.Parts.Any(t => t == position.Coordinates) || OutOfBounds(position)))
            {
                entity.GetFirst<Position>().Center();
                entity.GetFirst<Tail>().Parts.SetLength(0);
                SnakeMovement.MovementDirection = new Up();
            }
        }

        public void Execute(Entity primary, Entity secondary)
        {
            if (!primary.Has<Tail, Position>() || !secondary.Has<Apple, Position>())
                return;
            
            Position primaryPosition = primary.GetFirst<Position>();
            Position secondaryPosition = secondary.GetFirst<Position>();

            if (primaryPosition.Coordinates == secondaryPosition.Coordinates)
            {
                primary.GetFirst<Tail>().Enlarge();
                secondary.GetFirst<Position>().Random();
            }
        }

        private bool OutOfBounds(Position position)
        {
            return position.Coordinates.X < 0 || position.Coordinates.Y < 0 || position.Coordinates.X >= GameConfig.Width || position.Coordinates.Y >= GameConfig.Height;
        }
    }
}
