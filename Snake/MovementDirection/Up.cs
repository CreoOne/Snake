using System.Numerics;

namespace Snake.MovementDirection
{
    public class Up : IMovementDirection
    {
        public Vector2 Move(Vector2 currentPosition)
        {
            return new Vector2(currentPosition.X, currentPosition.Y -1);
        }
    }
}
