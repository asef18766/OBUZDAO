using Bolt;
using Resources;
using UnityEngine;

namespace Entity
{
    public class PickableItem : EntityBehaviour<IPickableItem>
    {
        public int itemId;
        public int amount;
        
        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = ItemManager.GetInstance().GetIcon(state.ItemId);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(BoltNetwork.IsClient)
                return;
            
            if(!other.CompareTag("Player")) return;
            var playerId = other.gameObject.GetComponent<Player>().playerId;
            BagManager.GetInstance().GetBag(playerId).InsertItem(itemId,amount);
            BoltNetwork.Destroy(gameObject);
        }
    }
}