using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
public static MainMenuManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void CreateLobby()
    {
        BootStrapMamager.CreateLobby();
    }
}
