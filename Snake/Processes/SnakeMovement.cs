﻿using EntityComponentFramework;
using EntityComponentFramework.Processes;
using EntityComponentFramework.Processes.Attributes;
using Snake.Components;
using Snake.MovementDirection;

namespace Snake.Processes
{
    public class SnakeMovement : IProcess
    {
        public static IMovementDirection MovementDirection = new Up();

        [FrequencyLimited(GameConfig.SnakeMovementInterval, false)]
        public void Execute(Entity entity)
        {
            if (!entity.Has<Tail, Position>())
                return;

            Position position = entity.GetFirst<Position>();
            Tail tail = entity.GetFirst<Tail>();

            tail.Parts.Add(position.Coordinates);
            position.Coordinates = MovementDirection.Move(position.Coordinates);

        }
    }
}
