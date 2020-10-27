using System;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public static class PlayerRegistry
    {
        private static Dictionary<uint, BoltEntity> _playerList = new Dictionary<uint, BoltEntity>();

        public static void CreatePlayer(BoltConnection connection)
        {
            var player = BoltNetwork.Instantiate(BoltPrefabs.PlayerPref, Vector3.zero, Quaternion.identity);
            player.AssignControl(connection);
            player.GetComponent<Entity.Player>().playerId = Convert.ToInt32(connection.ConnectionId);
            _playerList.Add(connection.ConnectionId, player);
        }

        public static void RemovePlayer(BoltConnection connection)
        {
            var playerId = connection.ConnectionId;
            if (_playerList.ContainsKey(playerId))
            {
                BoltNetwork.Destroy(_playerList[playerId]);
                _playerList.Remove(playerId);
            }
            else
            {
                BoltLog.Warn($"try to remove not existed player with connection id {playerId}");
            }
        }
    }
}