using EntityComponentFramework.Components;
using Snake.Snake;
using System;
using System.Linq;
using System.Numerics;

namespace Snake.Components
{
    public class Tail : IComponent
    {
        public DynamicLengthBuffer<Vector2> Parts { get; }
        public Tween<Vector2> Ending { get; set; }

        public Tail(TimeSpan tweenSpan, Func<double, Vector2, Vector2, Vector2> morph)
        {
            Parts = new DynamicLengthBuffer<Vector2>(0);
            Ending = new Tween<Vector2>(tweenSpan, morph, Vector2.Zero);
        }

        public void Enlarge()
        {
            Parts.SetLength(Parts.Count() + 1);
        }

        public void Add(Vector2 trace)
        {
            if(Parts.Count() == 0)
                Ending = new Tween<Vector2>(Ending.Span, Ending.Morph, trace);

            Parts.Add(trace);

            if(Parts.Count() > 0)
                Ending.Target = Parts.First();
        }
    }
}
