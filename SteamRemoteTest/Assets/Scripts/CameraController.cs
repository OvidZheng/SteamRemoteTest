using FishNet.Object;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            gameObject.SetActive(true);
        }
    }
}
