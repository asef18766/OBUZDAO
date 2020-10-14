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
    }
}