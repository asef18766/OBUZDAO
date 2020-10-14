using System.Collections.Generic;
using Bolt;
using UnityEngine;

namespace Networking
{
    public static class PlayerRegistry
    {
        private static Dictionary<IProtocolToken, object> _playerList = new Dictionary<IProtocolToken, object>();

        public static void CreatePlayer(BoltConnection connection)
        {
            var player = BoltNetwork.Instantiate(BoltPrefabs.PlayerPref, Vector3.zero, Quaternion.identity);
            player.AssignControl(connection);
            _playerList.Add(connection.ConnectToken, player);
        }
    }
}