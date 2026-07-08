using UnityEngine;

public class DROP : MonoBehaviour
{
    public enum FlyDirection { Up, Down, Left, Right }

    [Header("飞行设置")]
    [SerializeField] private FlyDirection direction = FlyDirection.Down;
    [SerializeField] private float flyForce = 10f;
    [SerializeField] private float destroyDelay = 2f;

    private Rigidbody2D rb;
    private bool isFlying = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 被 chufa.cs 触发后飞行
    public void TriggerFly()
    {
        if (isFlying) return;
        isFlying = true;

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // 确保刚体是 Dynamic 类型才能受力
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0; // 飞行过程中通常不受重力影响

        Vector2 forceVector = Vector2.zero;
        switch (direction)
        {
            case FlyDirection.Up: forceVector = Vector2.up; break;
            case FlyDirection.Down: forceVector = Vector2.down; break;
            case FlyDirection.Left: forceVector = Vector2.left; break;
            case FlyDirection.Right: forceVector = Vector2.right; break;
        }

        rb.linearVelocity = forceVector * flyForce;

        // 一段时间后自动销毁
        Destroy(gameObject, destroyDelay);
    }
}
