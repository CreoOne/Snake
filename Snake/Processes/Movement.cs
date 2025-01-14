﻿using EntityComponentFramework;
using EntityComponentFramework.Processes;
using EntityComponentFramework.Processes.Attributes;
using Snake.Components;
using Snake.Engine;
using Snake.MovementDirection;

namespace Snake.Processes
{
    public class Movement : IProcess
    {
        public static IMovementDirection MovementDirection = new Up();

        [FrequencyLimited(GameConfig.SnakeMovementInterval, false)]
        public void Move([Has(typeof(Tail), typeof(Position))] Entity entity)
        {
            if (GameConfig.Paused)
                return;

            Position position = entity.GetFirst<Position>();
            Tail tail = entity.GetFirst<Tail>();

            tail.Add(position.Coordinates.Target);
            position.Coordinates.Target = MovementDirection.Move(position.Coordinates.Target);
            StatisticsMonitor.IncrementTiles();
        }
    }
}
