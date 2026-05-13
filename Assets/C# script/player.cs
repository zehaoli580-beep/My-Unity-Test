using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;       // 2D刚体组件，用于处理物理运动
    private Animator anim;         // 动画器组件，用于控制角色动画
    private SpriteRenderer sprite; // 精灵渲染器组件，用于控制精灵翻转
    private BoxCollider2D coll;    // 碰撞器组件，用于地面检测
    private float dirX;            // 水平输入方向

    [SerializeField] private float moveSpeed = 7f;    // 移动速度
    [SerializeField] private float jumpForce = 7f;    // 跳跃力
    [SerializeField] private LayerMask jumpableGround; // 可跳跃地面层
    [SerializeField] private AudioSource jumpSound;   // 跳跃音效

    // 角色运动状态枚举
    private enum MovementState { stay, running, jump, fall }

    private void Start()
    {
        // 获取组件引用
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // 获取水平输入（使用新输入系统）
        var keyboard = Keyboard.current;

        dirX = 0f;
        if (keyboard != null)
        {
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            {
                dirX = 1f;
            }
            else if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            {
                dirX = -1f;
            }
        }

        // 跳跃输入检测（仅空格键）
        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
        {
            if (IsGrounded())
            {
                Jump();
            }
        }

        // 更新动画状态
        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        // 应用水平移动
        rb.linearVelocity = new Vector2(dirX * moveSpeed, rb.linearVelocity.y);
    }

    // 跳跃方法
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (jumpSound != null && jumpSound.clip != null)
        {
            jumpSound.Play();
        }
    }

    // 检测是否在地面
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    // 更新角色动画状态
    private void UpdateAnimationState()
    {
        MovementState state;

        // 根据水平输入判断是待机动画还是跑步动画
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false; // 向右移动，不翻转
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true; // 向左移动，翻转精灵
        }
        else
        {
            state = MovementState.stay; // 没有输入，待机动画
        }

        // 根据垂直速度判断跳跃或下落（优先级更高）
        if (rb.linearVelocity.y > 0.1f)
        {
            state = MovementState.jump; // 向上运动，跳跃动画
        }
        else if (rb.linearVelocity.y < -0.1f)
        {
            state = MovementState.fall; // 向下运动，下落动画
        }

        // 更新动画器的 state 参数
        anim.SetInteger("state", (int)state);
    }
}
