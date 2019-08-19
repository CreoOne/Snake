using System.Numerics;

namespace Snake.MovementDirection
{
    public interface IMovementDirection
    {
        Vector2 Move(Vector2 currentPosition);
    }
}
