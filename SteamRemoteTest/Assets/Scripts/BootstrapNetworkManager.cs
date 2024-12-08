using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Object;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using FishNet.Object;
using Heathen.SteamworksIntegration.API;
using UnityEngine;

namespace DefaultNamespace
{
    public class BootstrapNetworkManager : NetworkBehaviour
    {

        
        public static BootstrapNetworkManager instance;
        
        public NetworkConnection[] conns;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }
        
        public void changeNetworkScene(string sceneName, string[] scenesToClose)
        {
            instance.CloseScenes(scenesToClose);
            SceneLoadData sld = new SceneLoadData(sceneName);
            conns = instance.ServerManager.Clients.Values.ToArray();
            instance.SceneManager.LoadConnectionScenes(conns, sld);
        }
        
 
 
        [ServerRpc(RequireOwnership = false)]
        void CloseScenes(string[] scenesToClose)
        {
            CloseScenesObserver(scenesToClose);
        }
        
        [ObserversRpc]
        void CloseScenesObserver(string[] scenesToClose)
        {
            foreach (var scene in scenesToClose)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
}