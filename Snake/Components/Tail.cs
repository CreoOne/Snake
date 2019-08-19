using EntityComponentFramework.Components;
using Snake.Snake;
using System.Linq;
using System.Numerics;

namespace Snake.Components
{
    public class Tail : IComponent
    {
        public DynamicLengthBuffer<Vector2> Parts { get; set; } = new DynamicLengthBuffer<Vector2>(0);

        public void Enlarge()
        {
            Parts.SetLength(Parts.Count() + 1);
        }
    }
}
