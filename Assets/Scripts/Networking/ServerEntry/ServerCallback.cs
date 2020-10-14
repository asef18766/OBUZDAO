using System;
using Bolt;
using Bolt.Matchmaking;
using UnityEngine;

namespace Networking.ServerEntry
{
    public class ServerCallback : GlobalEventListener
    {
        public override void BoltStartDone()
        {
            BoltLog.Warn("set loading MainGame Scene~~");
            if (BoltNetwork.IsClient) return;
            var matchName = Guid.NewGuid().ToString();

            BoltMatchmaking.CreateSession(
                matchName,
                sceneToLoad: "MainGame"
            );
        }
    }
}