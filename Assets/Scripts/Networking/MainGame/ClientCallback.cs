using Entity;
using UnityEngine;

namespace Networking.MainGame
{
    [BoltGlobalBehaviour(BoltNetworkModes.Client,"MainGame")]
    public class ClientCallback : Bolt.GlobalEventListener
    {
        public override void Connected(BoltConnection connection)
        {
            Debug.LogWarning($"conn id : {connection.ConnectionId}");
        }
        public override void OnEvent(OnUpdateBagContent evnt)
        {
            BoltLog.Warn($"Client Receive bag content [{string.Join("," , Resources.Bag.DecodeBag(evnt.BagContent))}]");
        }

        public override void ControlOfEntityGained(BoltEntity entity)
        {
            var player = entity.GetComponent<Player>();
            if (player == null) return;
            if (Camera.main != null) Camera.main.transform.SetParent(player.gameObject.transform);
        public override void OnEvent(OnPlayerDeath evnt)
        {
            BoltLog.Warn($"player with id {evnt.DeathId} dead");
        }
    }
}