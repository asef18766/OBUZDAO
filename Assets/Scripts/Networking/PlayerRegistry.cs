using System.Collections.Generic;
using Bolt;
using UnityEngine;

namespace Networking
{
    public static class PlayerRegistry
    {
        private static Dictionary<uint, object> _playerList = new Dictionary<uint, object>();

        public static void CreatePlayer(BoltConnection connection)
        {
            var player = BoltNetwork.Instantiate(BoltPrefabs.PlayerPref, Vector3.zero, Quaternion.identity);
            //player.AssignControl(connection);
            _playerList.Add(connection.ConnectionId, player);
        }
    }
}