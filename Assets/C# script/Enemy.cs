using UnityEngine;

/// <summary>
/// 敌人AI脚本 - 实现简单的巡逻敌人
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("巡逻设置")]
    [SerializeField] private Transform pointA;       // 巡逻点A
    [SerializeField] private Transform pointB;       // 巡逻点B
    [SerializeField] private float moveSpeed = 2f;     // 移动速度
    [SerializeField] private float waitTime = 1f;      // 到达目标点后的等待时间

    [Header("伤害设置")]
    [SerializeField] private int damage = 1;           // 造成的伤害

    [Header("死亡设置")]
    [SerializeField] private float deathAnimDuration = 1f;  // 死亡动画时长

    private Vector3 targetPosition;
    private bool movingToB = true;
    private bool isWaiting = false;
    private float waitTimer;

    private Animator anim;
    private SpriteRenderer sprite;
    private bool isDead = false;
    private float deathTimer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        targetPosition = pointB.position;
    }

    private void Update()
    {
        if (isDead)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
            }
            return;
        }

        Patrol();
    }

    /// <summary>
    /// 巡逻逻辑
    /// </summary>
    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == pointB.position)
        {
            movingToB = false;
            targetPosition = pointA.position;
            isWaiting = true;
            waitTimer = waitTime;
            sprite.flipX = true;
        }
        else if (transform.position == pointA.position)
        {
            movingToB = true;
            targetPosition = pointB.position;
            isWaiting = true;
            waitTimer = waitTime;
            sprite.flipX = false;
        }
    }

    /// <summary>
    /// 触发检测 - 玩家碰到敌人时触发受伤或踩踏
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // 检查是否是从头顶踩下
                if (playerRb.linearVelocity.y < -0.1f)
                {
                    Die();
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 5f);
                }
                else
                {
                    // 普通碰撞，造成伤害
                    player playerScript = other.GetComponent<player>();
                    if (playerScript != null)
                    {
                        if (!playerScript.IsDefending())
                        {
                            playerScript.TakeDamage();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 敌人死亡 - 被玩家攻击或踩到时调用
    /// </summary>
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        deathTimer = deathAnimDuration;

        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }

        GameManager.Instance.AddScore();
    }
}
