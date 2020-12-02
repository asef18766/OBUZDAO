using System;
using System.Collections.Generic;
using Bolt;
using Entity;
using Resources;
using UnityEngine;

namespace Networking.MainGame
{
    [BoltGlobalBehaviour(BoltNetworkModes.Server, "MainGame")]
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

        public override void SceneLoadLocalBegin(string scene, IProtocolToken token)
        {
            BoltNetwork.Instantiate(BoltPrefabs.CrackableTerrian, new Vector3(4, 4, 0), Quaternion.identity);
            BoltNetwork.Instantiate(BoltPrefabs.CrackableTerrian, new Vector3(-7, 4, 0), Quaternion.identity).GetComponent<CrackableTerrain>().spawnItems = new List<ItemInfo>
            {
                new ItemInfo(0, 1)
            };
            BoltNetwork.Instantiate(BoltPrefabs.Archer, new Vector3(8, -4, 0), Quaternion.identity);
        }

        public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token)
        {
            BagManager.GetInstance().GetBag(Convert.ToInt32(connection.ConnectionId)).InsertItem(87, 3);
        }
    }
}