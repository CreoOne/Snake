using EntityComponentFramework;
using EntityComponentFramework.Processes;
using EntityComponentFramework.Processes.Attributes;
using Snake.Components;
using Snake.MovementDirection;
using System.Linq;
using System.Numerics;

namespace Snake.Processes
{
    public class Collision : IProcess
    {
        public void CollideSelf([Has(typeof(Tail), typeof(Position))] Entity entity)
        {
            Position position = entity.GetFirst<Position>();
            Tail tail = entity.GetFirst<Tail>();

            if (tail.Parts.Any(t => Collides(t, position.Coordinates.Target)) || OutOfBounds(position))
            {
                entity.GetFirst<Position>().TeleportCenter();
                entity.GetFirst<Tail>().Parts.SetLength(0);
                Movement.MovementDirection = new Up();
            }
        }

        public void CollideEntities([Has(typeof(Tail), typeof(Position))] Entity primary, [Has(typeof(Apple), typeof(Position))] Entity secondary)
        {
            Position primaryPosition = primary.GetFirst<Position>();
            Position secondaryPosition = secondary.GetFirst<Position>();

            if (Collides(primaryPosition.Coordinates.Target, secondaryPosition.Coordinates.Target))
            {
                primary.GetFirst<Tail>().Enlarge();
                secondary.GetFirst<Position>().MoveRandom();
            }
        }

        private bool OutOfBounds(Position position)
        {
            return position.Coordinates.Target.X < 0 || position.Coordinates.Target.Y < 0 || position.Coordinates.Target.X >= GameConfig.BoardWidth || position.Coordinates.Target.Y >= GameConfig.BoardHeight;
        }

        private bool Collides(Vector2 primary, Vector2 secondary)
        {
            return (primary - secondary).Length() < 1e-2f;
        }
    }
}
