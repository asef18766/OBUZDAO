using System;
using System.Collections.Generic;
using System.Linq;
using Networking;

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
        public uint OwnerId;
        private readonly bool _sendNotify = BoltNetwork.IsServer;
        
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
            if (_sendNotify)
                SendUpdateEvent();
        }

        public void RemoveItem(int id, int amount)
        {
            if (!_inventory.ContainsKey(id))
                throw new IndexOutOfRangeException($"try to remove not exsisted item with id {id} ");
            if (_inventory[id] < amount)
                throw new IndexOutOfRangeException($"try to remove too much item with id and amount {id} {amount}");
            _inventory[id] -= amount;
            if (_inventory[id] == 0)
                _inventory.Remove(id);
            if (_sendNotify)
                SendUpdateEvent();
        }

        public string EncodeBag()
        {
            return _inventory.Aggregate("", (current, it) => current + (Convert.ToChar(it.Key) + Convert.ToChar(it.Value)));
        }

        public static List<ItemInfo> DecodeBag(string encode)
        {
            var ret = new List<ItemInfo>();
            for (var i = 0; i < encode.Length; i+=2)
            {
                ret.Add(new ItemInfo(Convert.ToInt32(ret[i]) , Convert.ToInt32(ret[i+1])));
            }
            
            return ret;
        }
        private void SendUpdateEvent()
        {
            PlayerRegistry.GetPlayerRef(OwnerId).OnUpdateBag(EncodeBag());
        }
    }
}