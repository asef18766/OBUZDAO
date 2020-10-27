using System;
using System.Collections.Generic;
using System.Linq;

public struct ItemInfo
{
    public int Id;
    public int Amount;

    public ItemInfo(int id, int amount)
    {
        Id = id;
        Amount = amount;
    }
    public static bool operator <(ItemInfo info1, ItemInfo info2)
    {
        return info1.Id < info2.Id;
    }
    public static bool operator >(ItemInfo info1, ItemInfo info2)
    {
        return info1.Id > info2.Id;
    }
}
namespace Resources
{
    public class Bag
    {
        private SortedDictionary<int, int> _inventory = new SortedDictionary<int, int>();

        public List<ItemInfo> ListCurrentItem()
        {
            var ret = _inventory.Select(pair => new ItemInfo()).ToList();
            ret.Sort();
            return ret;
        }

        public void InsertItem(int id, int amount)
        {
            if (!_inventory.ContainsKey(id))
                _inventory.Add(id, 0);
            _inventory[id] += amount;
        }

        public void RemoveItem(int id, int amount)
        {
            if (!_inventory.ContainsKey(id))
                throw new IndexOutOfRangeException($"try to remove inexsistent item with id {id} ");
            if (_inventory[id] < amount)
                throw new IndexOutOfRangeException($"try to remove too much item with id and amount {id} {amount}");
            _inventory[id] -= amount;
            if (_inventory[id] == 0)
                _inventory.Remove(id);
        }
    }
}