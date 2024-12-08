using System;
using DefaultNamespace;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class Gamemode : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPlayer(BootstrapNetworkManager.instance.conns);
    }

    private void SpawnPlayer(NetworkConnection[] conns)
    {
        foreach (var conn in conns)
        {
            GameObject player = Instantiate(playerPrefab);
            InstanceFinder.ServerManager.Spawn(player, conn);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Big Mouse!!");
        }
    }
}
