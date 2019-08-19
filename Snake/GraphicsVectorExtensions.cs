using System.Drawing;
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
    }
}
