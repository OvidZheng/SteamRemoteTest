using System;
using FishNet.Managing;
using Heathen.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrapMamager : MonoBehaviour
{
    private static BootStrapMamager _instance;
    
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;
    [SerializeField] private string menuSceneName;


    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> LobbyEntered;
    
    public static ulong currentLobbyId;
    public static void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
    }
    
    public static void JoinById(CSteamID steamID)
    {
        Debug.Log("LOG: Joining lobby by id" + steamID);
        if(SteamMatchmaking.RequestLobbyData(steamID))
        {
            Debug.Log("LOG: Requested lobby data: " + steamID);
            SteamMatchmaking.JoinLobby(steamID);
        }
        else
        {
            Debug.Log("LOG: Failed to request lobby data: " + steamID);
        }
    }
    
    public static void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(currentLobbyId));
        currentLobbyId = 0;
        _instance._fishySteamworks.StopConnection(false);
        if(_instance._networkManager.IsServer)
        {
            _instance._fishySteamworks.StopConnection(true);
        }
    }
    
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        
        App.evtSteamInitialized.AddListener(GoToMenu);
    }

    private void Start()
    {
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Additive);
    }
    
    private void OnLobbyCreated(LobbyCreated_t pCallback)
    {
        if (pCallback.m_eResult == EResult.k_EResultOK)
        {
            Debug.Log("LOG: Lobby created successfully: " + pCallback.m_ulSteamIDLobby + " " + pCallback.m_eResult);
        }
        else
        {
            Debug.Log("LOG: Lobby created failed " + pCallback.m_eResult);
        }
        
        currentLobbyId = pCallback.m_ulSteamIDLobby;
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyId), "HostAddress", SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyId), "name", SteamFriends.GetPersonaName() + "'s lobby");
        _fishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        _fishySteamworks.StartConnection(true);
        Debug.Log("- Lobby created!!");
    }
    
    private void OnJoinRequested(GameLobbyJoinRequested_t pCallback)
    {
        Debug.Log("LOG: Join requested");
        SteamMatchmaking.JoinLobby(pCallback.m_steamIDLobby);
    }
    
    private void OnLobbyEntered(LobbyEnter_t pCallback)
    {
        if (pCallback.m_EChatRoomEnterResponse == 1)
        {
            Debug.Log("LOG: Lobby entered successfully");
        }
        else
        {
            Debug.Log("LOG: Lobby entered failed");
        }
        
        currentLobbyId = pCallback.m_ulSteamIDLobby;
        MainMenuManager.LobbyEntered(SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyId), "name"), _networkManager.IsServer);

        _fishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyId), "HostAddress"));
        _fishySteamworks.StartConnection(false);
    }
    
}
