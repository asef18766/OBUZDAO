using System;
using Bolt;
using Resources;

namespace Networking.MainGame
{
    [BoltGlobalBehaviour(BoltNetworkModes.Server,"MainGame")]
    public class ServerCallback : GlobalEventListener
    {
        public override void Connected(BoltConnection connection)
        {
            PlayerRegistry.CreatePlayer(connection);
        }

        public override void Disconnected(BoltConnection connection)
        {
            PlayerRegistry.RemovePlayer(connection);
        }

        public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token)
        {
            BagManager.GetInstance().GetBag(Convert.ToInt32(connection.ConnectionId) ).InsertItem(87, 3);
        }
    }
}