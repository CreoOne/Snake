using EntityComponentFramework;
using EntityComponentFramework.Processes;
using EntityComponentFramework.Processes.Attributes;
using Snake.Components;
using Snake.MovementDirection;

namespace Snake.Processes
{
    public class Movement : IProcess
    {
        public static IMovementDirection MovementDirection = new Up();

        [FrequencyLimited(GameConfig.SnakeMovementInterval, false)]
        public void Move([Has(typeof(Tail), typeof(Position))] Entity entity)
        {
            Position position = entity.GetFirst<Position>();
            Tail tail = entity.GetFirst<Tail>();

            tail.Parts.Add(position.Coordinates);
            position.Coordinates = MovementDirection.Move(position.Coordinates);

        }
    }
}
