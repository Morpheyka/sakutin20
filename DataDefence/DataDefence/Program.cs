using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDefence
{
    class Bag
    {
        public IEnumerable<IReadOnlyItem> Items => _items;
        private readonly List<Item> _items = new List<Item>();
        private readonly uint _maxWeidth;

        public Bag(uint maxWeidth) =>
            _maxWeidth = maxWeidth;

        public void AddItem(Item item, int count)
        {
            _ = item
                ?? throw new InvalidOperationException();

            if (FitInBag(count) == false)
                throw new InvalidOperationException();

            if (Contains(item) == false)
                _items.Add(item);

            item.count += count;
        }

        private bool Contains(Item item) =>
            _items.FirstOrDefault(targetItem => targetItem == item) != null;

        private bool FitInBag(int weight) =>
            GetCurrentWeight() + weight < _maxWeidth;

        private int GetCurrentWeight() => _items.Sum(item => item.count);
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
