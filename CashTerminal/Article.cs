using System;

namespace CashTerminal
{
    class Article
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public float Price { get; private set; }
        public int MaxDiscount { get; private set; }
        public string Measure { get; private set; }

        public float Count
        {
            get { return Count; }
            set
            {
                if (value > 0)
                    Count = value;
            }
        }
    }
}
