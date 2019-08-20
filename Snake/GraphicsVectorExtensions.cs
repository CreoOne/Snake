using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Snake
{
    public static class GraphicsVectorExtensions
    {
        public static void FillRectangle(this Graphics self, Brush brush, Vector2 origin, Vector2 size)
        {
            self.FillRectangle(brush, origin.X, origin.Y, size.X, size.Y);
        }

        public static void FillEllipse(this Graphics self, Brush brush, Vector2 origin, Vector2 size)
        {
            self.FillEllipse(brush, origin.X, origin.Y, size.X, size.Y);
        }

        public static void DrawLines(this Graphics self, Pen pen, IEnumerable<Vector2> vertexes)
        {
            List<Vector2> vertexList = new List<Vector2>(vertexes);
            float offset = pen.Width / 2f;

            foreach (int index in Enumerable.Range(0, vertexList.Count - 1))
                self.DrawLine(pen, vertexList[index].X + offset, vertexList[index].Y + offset, vertexList[index + 1].X + offset, vertexList[index + 1].Y + offset);
        }
    }
}
