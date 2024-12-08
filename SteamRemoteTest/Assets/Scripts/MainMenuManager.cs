using System;
using DefaultNamespace;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }
    
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject LobbyMenu;
    [SerializeField] private TMP_InputField lobbyInput;
    [SerializeField] private TextMeshProUGUI lobbyTitle;
    [SerializeField] private TextMeshProUGUI lobbyIdText;
    [SerializeField] private Button startGameButton;
    [SerializeField] string[] scemesToClose;
    [SerializeField] string GameSceneName;
    [SerializeField] GameObject debugConsole;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        OpenMainMenu();
    }

    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        Instance.lobbyTitle.text = lobbyName;
        Instance.lobbyIdText.text = "LobbyId: " + BootStrapMamager.currentLobbyId.ToString();
        Instance.startGameButton.gameObject.SetActive(isHost);
        Instance.OpenLobbyMenu();
    }
    public void CreateLobby()
    {
        BootStrapMamager.CreateLobby();
    }
    
    public void JoinLobby()
    {
        CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
        BootStrapMamager.JoinById(steamID);
    }
    
    public void LeaveLobby()
    {
        BootStrapMamager.LeaveLobby();
        OpenMainMenu();
    }

    public void StartGame()
    {
        BootstrapNetworkManager.instance.changeNetworkScene(GameSceneName,scemesToClose);
    }
    
    public void OpenMainMenu()
    {
        CloseAllMenus();
        MainMenu.SetActive(true);
    }
    
    public void CloseAllMenus()
    {
        MainMenu.SetActive(false);
        LobbyMenu.SetActive(false);
    }
    
    public void OpenLobbyMenu()
    {
        CloseAllMenus();
        LobbyMenu.SetActive(true);
    }


}
