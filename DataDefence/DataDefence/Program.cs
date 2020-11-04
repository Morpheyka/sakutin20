using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDefence
{
    class Bag
    {
        public IEnumerable<IReadOnlyItem> Items => _items;

        private readonly List<Item> _items = new List<Item>();
        private readonly uint _maxWeidth = 0;

        public Bag(uint maxWeidth)
        {
            _maxWeidth = maxWeidth;
        }

        public void AddItem(Item item, int count)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (FitInBag(count) == false)
                throw new InvalidOperationException();

            if (TryGetItemByName(item.Name, out Item exist))
                exist.Count += count;
            else
                _items.Add(new Item(item.Count, item.Name));
        }

        private bool TryGetItemByName(string name, out Item result)
        {
            result = _items.FirstOrDefault(targetItem => targetItem.Name == name);

            return result != null;
        }

        private bool FitInBag(int weight)
        {
            return GetCurrentWeight() + weight < _maxWeidth;
        }

        private int GetCurrentWeight()
        {
            return _items.Sum(item => item.Count);
        }
    }

    class Item : IReadOnlyItem
    {
        public readonly string Name = string.Empty;
        public int Count = 0;

        public Item(int count, string name)
        {
            (Name, Count) = (name, count);
        }

        int IReadOnlyItem.Count => Count;
        string IReadOnlyItem.Name => Name;
    }

    interface IReadOnlyItem
    {
        int Count { get; }
        string Name { get; }
    }
}
