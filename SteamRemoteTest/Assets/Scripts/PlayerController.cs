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
        if(!IsClient)
        {
            return;
        }
        
        if (!IsOwner)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            return;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        // 获取玩家输入
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition((Vector2) transform.position + movement.normalized * moveSpeed * Time.deltaTime);
        //rb.linearVelocity = movement.normalized * moveSpeed;
    }

}

