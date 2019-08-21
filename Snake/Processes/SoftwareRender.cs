using EntityComponentFramework;
using EntityComponentFramework.Processes;
using EntityComponentFramework.Processes.Attributes;
using Snake.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
            context.SmoothingMode = SmoothingMode.AntiAlias;
        }

        [BeforeAll]
        public void Clear()
        {
            Context.Clear(Color.White);
        }

        public void Render(Entity entity)
        {
            if (entity.Has<Tail, Position>())
            {
                float tailSize = Math.Min(TileSize.X, TileSize.Y);
                Position position = entity.GetFirst<Position>();
                Tail tail = entity.GetFirst<Tail>();

                using (SolidBrush brush = new SolidBrush(Color.Black))
                using (Pen pen = new Pen(Color.Black, tailSize))
                {
                    pen.StartCap = pen.EndCap = LineCap.Round;

                    if (tail.Parts.Count() > 0)
                    {
                        IEnumerable<Vector2> trace = Enumerable
                            .Append(tail.Parts, position.Coordinates.Current)
                            .Prepend(tail.Ending.Current);

                        Context.DrawLines(pen, trace.Select(c => c * tailSize));
                    }

                    else
                        Context.FillEllipse(brush, position.Coordinates.Current * TileSize, new Vector2(tailSize));
                }
            }

            if (entity.Has<Apple, Position>())
            {
                Position position = entity.GetFirst<Position>();
                using (SolidBrush brush = new SolidBrush(Color.Red))
                    Context.FillEllipse(brush, position.Coordinates.Current * TileSize + Margin, AppleSize);
            }
        }

        [AfterAll]
        public void Finish()
        {
            ExecuteEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}
