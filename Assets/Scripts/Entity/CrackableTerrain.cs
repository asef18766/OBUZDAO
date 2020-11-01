using System.Collections.Generic;
using Bolt;
using UnityEngine;

namespace Entity
{
    public class CrackableTerrain : EntityBehaviour<ICrackableTerrain> , IDestroyable
    {
        public int health;
        public List<ItemInfo> spawnItems;

        public void OnHurt(int dmg)
        {
            BoltLog.Warn($"is server:{BoltNetwork.IsServer}");
            health -= dmg;
            if (health >= 0) return;
            
            if (spawnItems != null)
            {
                BoltLog.Warn($"spawning leftover");
                foreach (var it in spawnItems)
                {
                    var spawn = BoltNetwork.Instantiate(BoltPrefabs.PickableItem, transform.position, Quaternion.identity).GetComponent<PickableItem>();
                    spawn.itemId = it.Id;
                    spawn.amount = it.Amount;
                }
            }
            BoltNetwork.Destroy(gameObject);
        }
    }
}