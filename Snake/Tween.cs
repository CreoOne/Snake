using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Tween<T>
    {
        public T Current
        {
            get
            {
                return Morph(GetPosition(), Origin, Target);
            }
        }

        private T _target { get; set; }
        public T Target
        {
            get
            {
                return _target;
            }

            set
            {
                Origin = Current;
                _target = value;
                From = DateTime.UtcNow;
            }
        }

        public T Origin { get; private set; }
        public TimeSpan Span { get; }
        private DateTime From { get; set; }
        public Func<double, T, T, T> Morph { get; private set; }

        public Tween(TimeSpan span, Func<double, T, T, T> morph, T start)
        {
            Morph = morph;
            Span = span;
            Origin = Target = start;
            From = DateTime.UtcNow + Span;
        }

        private double GetPosition()
        {
            return Math.Max(0, Math.Min(1, (DateTime.UtcNow - From).TotalMilliseconds / Span.TotalMilliseconds));
        }
    }
}
