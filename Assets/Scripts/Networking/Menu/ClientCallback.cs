using System;
using Bolt.Matchmaking;
using UdpKit;
using UnityEngine;

namespace Networking.Menu
{
    public class ClientCallback : Bolt.GlobalEventListener
    {
        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            foreach (var session in sessionList)
            {
                var photonSession = session.Value;

                if (photonSession.Source == UdpSessionSource.Photon)
                {
                    BoltMatchmaking.JoinSession(photonSession);
                }
            }
        }

        public void JoinGame()
        {
            BoltLauncher.StartClient();
            Debug.LogWarning("client started~~");
        }
        
    }
}