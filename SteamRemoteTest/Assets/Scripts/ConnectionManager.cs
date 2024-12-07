using System;
using FishNet.Transporting;
using Heathen.SteamworksIntegration;
using TMPro;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    private static ConnectionManager instance;
    
    [SerializeField] private TMP_InputField connectionInput;
    [SerializeField] private TextMeshProUGUI   HostAddresText;
    [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;
    [SerializeField] private TextMeshProUGUI _successDisplay;
    
    private string _hostHex;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        
        // 订阅连接成功的事件
        _fishySteamworks.OnClientConnectionState += HandleConnectionStateChange;
        
        DontDestroyOnLoad(this);
    }
    
    void OnDestroy()
    {
        // 确保在对象销毁时取消订阅事件
        if (_fishySteamworks != null)
        {
            _fishySteamworks.OnClientConnectionState -= HandleConnectionStateChange;
        }
    }


    public void StartHost()
    {
        var user = UserData.Get();
        _hostHex = user.ToString();

        _fishySteamworks.StartConnection(true);
        _fishySteamworks.StartConnection(false);

        HostAddresText.text = "Host Addr: " + instance._hostHex;
    }

    public void StartClient()
    {
        _hostHex = connectionInput.text;
        var hostUser = UserData.Get(_hostHex);

        if (!hostUser.IsValid)
        {
            Debug.Log("HostUser InValid!");
            return;
        }
        
        _fishySteamworks.SetClientAddress(hostUser.id.ToString());
        _fishySteamworks.StartConnection(false);
    }

    public static string GetHostHex()
    {
        return instance._hostHex;
    }
    
    private void HandleConnectionStateChange(ClientConnectionStateArgs args)
    {
        switch (args.ConnectionState)
        {
            case LocalConnectionState.Started:
                Debug.Log("Connection successful!");
                OnConnectionSuccessful();
                break;

            case LocalConnectionState.StoppedError:
                Debug.LogError("Connection failed!");
                OnConnectionFailed();
                break;

            default:
                // 忽略其他状态
                break;
        }
    }

    private void OnConnectionSuccessful()
    {
        // 连接成功后的逻辑
        // 例如：加载下一场景或显示成功提示
        Debug.Log("Performing actions after successful connection...");
        _successDisplay.transform.gameObject.SetActive(true);
    }

    private void OnConnectionFailed()
    {
        // 连接失败后的逻辑
        // 例如：提示用户、重新尝试连接等
        Debug.Log("Performing actions after failed connection...");
    }
 
}
