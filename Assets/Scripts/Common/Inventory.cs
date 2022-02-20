using System;
using System.Collections.Generic;
using TestProject.Resources;

namespace TestProject.Common
{
    public sealed class Inventory
    {
        private readonly List<Item> _items = new List<Item>();

        public event Action<int, int> OnItemsCountChanged;
        public event Action<Item> OnItemAdded;
        
        public IReadOnlyList<Item> Items => _items;

        public void AddItem(Item item)
        {
            _items.Add(item);
            OnItemsCountChanged?.Invoke(_items.Count - 1, _items.Count);
            OnItemAdded?.Invoke(item);
        }
    }
}