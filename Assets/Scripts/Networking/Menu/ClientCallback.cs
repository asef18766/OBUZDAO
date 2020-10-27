using System;
using System.Collections;
using Bolt.Matchmaking;
using UdpKit;
using UnityEngine;
using UnityEngine.UI;

namespace Networking.Menu
{
    public class ClientCallback : Bolt.GlobalEventListener
    {
        [SerializeField] private GameObject loadingPanel = null;
        [SerializeField] private GameObject timeOutPanel = null;
        [SerializeField] private float waitTime = 5.0f;

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

        private IEnumerator Retry()
        {
            loadingPanel.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            BoltLauncher.Shutdown();
            loadingPanel.SetActive(false);
            timeOutPanel.SetActive(true);
        }
        public void JoinGame()
        {
            BoltLauncher.StartClient();
            Debug.LogWarning("client started~~");
            StartCoroutine(Retry());
        }
        
    }
}