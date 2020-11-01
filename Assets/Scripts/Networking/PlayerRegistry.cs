using System;
using System.Collections.Generic;
using Entity;
using Resources;
using UnityEngine;

namespace Networking
{
    public static class PlayerRegistry
    {
        private static readonly Dictionary<uint, BoltEntity> PlayerList = new Dictionary<uint, BoltEntity>();
        private static readonly BagManager BagManager = BagManager.GetInstance();
        public static void CreatePlayer(BoltConnection connection)
        {
            var player = BoltNetwork.Instantiate(BoltPrefabs.PlayerPref, Vector3.zero, Quaternion.identity);
            PlayerList.Add(connection.ConnectionId, player);
            player.AssignControl(connection);
            
            var playerScript = player.GetComponent<Player>();
            playerScript.playerId = Convert.ToInt32(connection.ConnectionId);
            BagManager.CreatePlayerBag(playerScript.playerId);
            BagManager.GetBag(playerScript.playerId).InsertItem(87, 3);
        }

        public static void RemovePlayer(BoltConnection connection)
        {
            var playerId = connection.ConnectionId;
            if (PlayerList.ContainsKey(playerId))
            {
                BagManager.RemovePlayerBag(Convert.ToInt32(playerId));
                BoltNetwork.Destroy(PlayerList[playerId]);
                PlayerList.Remove(playerId);
            }
            else
            {
                BoltLog.Warn($"try to remove not existed player with connection id {playerId}");
            }
        }

        public static Player GetPlayerRef(uint playerId)
        {
            if (!PlayerList.ContainsKey(playerId))
                throw new IndexOutOfRangeException(
                    $"try to access player reference with not existed player id {playerId}");
            
            return PlayerList[playerId].GetComponent<Player>();
        }
    }
}