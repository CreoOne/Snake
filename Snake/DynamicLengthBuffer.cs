using System.Collections;
using System.Collections.Generic;

namespace Snake.Snake
{
    public sealed class DynamicLengthBuffer<TItem> : IEnumerable<TItem>
    {
        private Queue<TItem> Items { get; set; }
        private int Length { get; set; }

        public DynamicLengthBuffer(int initialLenght)
        {
            Items = new Queue<TItem>();
            SetLength(initialLenght);
        }

        public void Add(TItem item)
        {
            Items.Enqueue(item);
            Trim();
        }

        public void SetLength(int lenght)
        {
            if (lenght < 0)
                return;

            Length = lenght;
            Trim();
        }

        private void Trim()
        {
            while (Items.Count > Length)
                Items.Dequeue();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
