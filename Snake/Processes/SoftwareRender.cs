using EntityComponentFramework;
using EntityComponentFramework.Processes;
using EntityComponentFramework.Processes.Attributes;
using Snake.Components;
using System;
using System.Drawing;
using System.Numerics;

namespace Snake.Processes
{
    [FrequencyLimited(16, true)]
    public class SoftwareRender : IProcess
    {
        private static readonly Vector2 TileSize = new Vector2(GameConfig.TileWidth, GameConfig.TileHeight);
        private static readonly Vector2 Margin = TileSize / 4;
        private static readonly Vector2 AppleSize = TileSize / 2;
        private readonly Graphics Context;
        public event EventHandler ExecuteEnded;

        public SoftwareRender(Graphics context)
        {
            Context = context;
        }

        [BeforeAll]
        public void Clear()
        {
            Context.Clear(Color.White);
        }

        public void Render(Entity entity)
        {
            using (SolidBrush brush = new SolidBrush(Color.Black))
                if(entity.Has<Tail, Position>())
                { 
                    Position position = entity.GetFirst<Position>();
                    Context.FillRectangle(brush, position.Coordinates * TileSize, TileSize);

                    Tail snake = entity.GetFirst<Tail>();
                    foreach(Vector2 bodyPosition in snake.Parts)
                        Context.FillRectangle(brush, bodyPosition * TileSize, TileSize);
                }

            using (SolidBrush brush = new SolidBrush(Color.Red))
                if(entity.Has<Apple, Position>())
                {
                    Position position = entity.GetFirst<Position>();
                    Context.FillEllipse(brush, position.Coordinates * TileSize + Margin, AppleSize);
                }

        }

        [AfterAll]
        public void Finish()
        {
            ExecuteEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}
