using System.Numerics;

namespace Snake.MovementDirection
{
    public class Left : IMovementDirection
    {
        public Vector2 Move(Vector2 currentPosition)
        {
            return new Vector2(currentPosition.X - 1, currentPosition.Y);
        }
    }
}
