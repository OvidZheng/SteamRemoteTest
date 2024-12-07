using UnityEngine;
using FishNet.Object;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f; // 玩家移动速度
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        // 获取 Rigidbody2D 组件
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        // 获取玩家输入
        movement.x = Input.GetAxisRaw("Horizontal"); // 水平输入（A/D 或 左/右方向键）
        movement.y = Input.GetAxisRaw("Vertical"); // 垂直输入（W/S 或 上/下方向键）
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

}

