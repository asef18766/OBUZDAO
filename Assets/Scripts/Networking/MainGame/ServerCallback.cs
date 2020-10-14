using System;
using Bolt.Matchmaking;
using UnityEngine;

namespace Networking.MainGame
{
    [BoltGlobalBehaviour(BoltNetworkModes.Server,"MainGame")]
    public class ServerCallback : Bolt.GlobalEventListener
    {
        public override void Connected(BoltConnection connection)
        {
            PlayerRegistry.CreatePlayer(connection);
        }
    }
}