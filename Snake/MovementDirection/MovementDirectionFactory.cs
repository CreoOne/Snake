using System.Windows.Forms;

namespace Snake.MovementDirection
{
    public class MovementDirectionFactory
    {
        public static IMovementDirection Create(Keys keys)
        {
            switch (keys)
            {
                case Keys.A:
                case Keys.Left: return new Left();

                case Keys.D:
                case Keys.Right: return new Right();

                case Keys.W:
                case Keys.Up: return new Up();

                case Keys.S:
                case Keys.Down: return new Down();

                default: return null;
            }
        }
    }
}
