using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDefence
{
    class Bag
    {
        public IEnumerable<IReadOnlyItem> Items => _items;
        public readonly uint maxWeidth;
        private readonly List<Item> _items = new List<Item>();

        public Bag(uint maxWeidth) =>
            this.maxWeidth = maxWeidth;

        public void AddItem(string name, int count)
        {
            int currentWeidth = _items.Sum(item => item.count);
            Item targetItem = _items.FirstOrDefault(item => item.name == name);

            _ = targetItem
                ?? throw new InvalidOperationException();

            targetItem.count = currentWeidth + count > maxWeidth
                ? throw new InvalidOperationException()
                : targetItem.count + count;
        }
    }

    class Item : IReadOnlyItem
    {
        public readonly string name;
        public int count;

        public Item(int count, string name) =>
            (this.name, this.count) = (name, count);

        int IReadOnlyItem.Count => count;
        string IReadOnlyItem.Name => name;
    }

    interface IReadOnlyItem
    {
        int Count { get; }
        string Name { get; }
    }
}