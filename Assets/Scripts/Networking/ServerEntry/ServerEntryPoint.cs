using UnityEngine;

namespace Networking.ServerEntry
{
    public class ServerEntryPoint : MonoBehaviour
    {
        [SerializeField] private bool isServer = false;

        private void Awake()
        {
            if(!isServer) return;
            if(BoltNetwork.IsRunning)
                BoltLauncher.Shutdown();
            BoltLauncher.StartServer();
            Debug.LogWarning("start server~~");
        }
    }
}