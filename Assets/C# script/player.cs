using UnityEngine;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private float dirX;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource jumpSound;

    [Header("二段跳设置")]
    [SerializeField] private bool enableDoubleJump = true;
    [SerializeField] private float doubleJumpForce = 6f;
    private int jumpCount = 0;
    private int maxJumpCount = 1;

    private enum MovementState { stay, running, jump, fall, hit, def, cast }
    
    [Header("攻击设置")]
    [SerializeField] private bool enableAttack = true;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackCooldownTime = 0.5f;
    [SerializeField] private CooldownUI attackCooldownUI;
    private bool isAttacking = false;
    private float attackDuration = 0.3f;
    private float attackTimer = 0f;
    private float attackCooldownTimer = 0f;
    private bool isAttackOnCooldown = false;
    
    [Header("防御设置")]
    [SerializeField] private bool enableDefense = true;
    [SerializeField] private float defenseDuration = 0.3f;
    [SerializeField] private float defenseCooldownTime = 0.3f;
    [SerializeField] private CooldownUI defenseCooldownUI;
    private bool isDefending = false;
    private float defenseTimer = 0f;
    private float defenseCooldownTimer = 0f;
    private bool isDefenseOnCooldown = false;
    
    [Header("技能设置")]
    [SerializeField] private bool enableSkill = true;
    [SerializeField] private float castDuration = 0.5f;
    [SerializeField] private float castCooldownTime = 1f;
    [SerializeField] private CooldownUI castCooldownUI;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballSpawnOffset = 1f;
    [SerializeField] private LayerMask groundLayer;
    private bool isCasting = false;
    private float castTimer = 0f;
    private float castCooldownTimer = 0f;
    private bool isCastOnCooldown = false;
    
    [Header("死亡设置")]
    private bool isDead = false;
    private int deathCount = 0;
    private float deathAnimationDuration = 1f;
    private bool isPlayingDeathAnimation = false;
    
    [Header("受伤设置")]
    private bool isHurt = false;
    private float hurtDuration = 0.5f;
    private float hurtTimer = 0f;
    private bool isInvincible = false;
    private float invincibleDuration = 1.5f;
    private float invincibleTimer = 0f;
    
    [Header("闪烁效果设置")]
    private float blinkInterval = 0.1f;
    private float blinkTimer = 0f;
    private bool isBlinking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isPlayingDeathAnimation)
        {
            deathAnimationDuration -= Time.deltaTime;
            if (deathAnimationDuration <= 0f)
            {
                isPlayingDeathAnimation = false;
                deathAnimationDuration = 1f;
                if (deathCount < 3)
                {
                    Respawn();
                }
                else
                {
                    GameManager.Instance.LoadFailureScene();
                }
            }
            UpdateAnimationState();
            return;
        }

        UpdateTimers();
        UpdateCooldowns();

        if (enableAttack && Input.GetKeyDown(KeyCode.J))
        {
            if (!isAttacking && !isDefending && !isHurt && !isCasting && !isAttackOnCooldown)
            {
                Attack();
            }
        }

        if (enableDefense && Input.GetKeyDown(KeyCode.K))
        {
            if (!isAttacking && !isDefending && !isHurt && !isCasting && !isDefenseOnCooldown)
            {
                Defend();
            }
        }
        
        if (enableSkill && Input.GetKeyDown(KeyCode.L))
        {
            if (!isAttacking && !isDefending && !isHurt && !isCasting && !isCastOnCooldown)
            {
                CastSkill();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking && !isDefending && !isHurt && !isCasting)
            {
                if (IsGrounded())
                {
                    Jump();
                    jumpCount = 1;
                }
                else if (enableDoubleJump && jumpCount < maxJumpCount)
                {
                    DoubleJump();
                    jumpCount++;
                }
            }
        }

        if (isAttacking || isDefending || isHurt || isCasting)
        {
            UpdateAnimationState();
            return;
        }

        dirX = 0f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dirX = 1f;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dirX = -1f;
        }

        if (IsGrounded())
        {
            jumpCount = 0;
        }

        UpdateAnimationState();
    }

    private void UpdateTimers()
    {
        if (isHurt)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0f)
            {
                isHurt = false;
            }
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }
        
        if (isDefending)
        {
            defenseTimer -= Time.deltaTime;
            if (defenseTimer <= 0f)
            {
                isDefending = false;
            }
        }
        
        if (isCasting)
        {
            castTimer -= Time.deltaTime;
            if (castTimer <= 0f)
            {
                isCasting = false;
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            isBlinking = true;
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                sprite.enabled = !sprite.enabled;
                blinkTimer = blinkInterval;
            }
            
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                isBlinking = false;
                sprite.enabled = true;
            }
        }
    }

    private void UpdateCooldowns()
    {
        if (isAttackOnCooldown)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0f)
            {
                isAttackOnCooldown = false;
                attackCooldownTimer = 0f;
            }
        }

        if (isDefenseOnCooldown)
        {
            defenseCooldownTimer -= Time.deltaTime;
            if (defenseCooldownTimer <= 0f)
            {
                isDefenseOnCooldown = false;
                defenseCooldownTimer = 0f;
            }
        }

        if (isCastOnCooldown)
        {
            castCooldownTimer -= Time.deltaTime;
            if (castCooldownTimer <= 0f)
            {
                isCastOnCooldown = false;
                castCooldownTimer = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isPlayingDeathAnimation)
        {
            return;
        }
        
        rb.linearVelocity = new Vector2(dirX * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (jumpSound != null && jumpSound.clip != null)
        {
            jumpSound.Play();
        }
    }

    private void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        if (jumpSound != null && jumpSound.clip != null)
        {
            jumpSound.Play();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        attackTimer = attackDuration;
        isAttackOnCooldown = true;
        attackCooldownTimer = attackCooldownTime;
        
        if (attackCooldownUI != null)
        {
            attackCooldownUI.StartCooldown(attackCooldownTime);
        }
        
        Vector2 attackDirection = sprite.flipX ? Vector2.left : Vector2.right;
        Vector2 attackOrigin = (Vector2)transform.position + attackDirection * 0.8f;
        RaycastHit2D hit = Physics2D.Raycast(attackOrigin, attackDirection, attackRange, enemyLayer);
        
        Debug.Log("Attack! Direction: " + attackDirection + ", Hit: " + (hit.collider != null ? hit.collider.name : "None"));
        
        if (hit.collider != null)
        {
            Debug.Log("Hit enemy: " + hit.collider.gameObject.name);
            Enemy enemyScript = hit.collider.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Die();
            }
            else
            {
                Destroy(hit.collider.gameObject);
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore();
            }
        }
    }

    private void Defend()
    {
        isDefending = true;
        defenseTimer = defenseDuration;
        isDefenseOnCooldown = true;
        defenseCooldownTimer = defenseCooldownTime;
        
        if (defenseCooldownUI != null)
        {
            defenseCooldownUI.StartCooldown(defenseCooldownTime);
        }
    }
    
    private void CastSkill()
    {
        isCasting = true;
        castTimer = castDuration;
        isCastOnCooldown = true;
        castCooldownTimer = castCooldownTime;
        
        if (castCooldownUI != null)
        {
            castCooldownUI.StartCooldown(castCooldownTime);
        }
        
        if (fireballPrefab != null)
        {
            Vector2 direction = sprite.flipX ? Vector2.left : Vector2.right;
            Vector2 spawnPosition = (Vector2)transform.position + direction * fireballSpawnOffset;
            
            GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
            Fireball fireballScript = fireball.GetComponent<Fireball>();
            
            if (fireballScript != null)
            {
                fireballScript.Initialize(direction);
            }
        }
    }

    /// <summary>
    /// 检查玩家是否在防御状态
    /// </summary>
    public bool IsDefending()
    {
        return isDefending;
    }

    public void TakeDamage()
    {
        if (isInvincible || isPlayingDeathAnimation) return;
        PlayDeathAnimation();
    }
    
    private void PlayDeathAnimation()
    {
        deathCount++;
        
        PlayerDeath playerDeath = GetComponent<PlayerDeath>();
        if (playerDeath != null)
        {
            playerDeath.Die();
        }
        
        isPlayingDeathAnimation = true;
        deathAnimationDuration = 1f;
        dirX = 0f;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    private void Respawn()
    {
        isPlayingDeathAnimation = false;
        deathAnimationDuration = 1f;
        
        if (GameManager.Instance != null)
        {
            transform.position = GameManager.Instance.GetPlayerStartPosition();
        }
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = false;
        }
        
        PlayerDeath playerDeath = GetComponent<PlayerDeath>();
        if (playerDeath != null)
        {
            playerDeath.ResetDeath();
        }
        
        if (anim != null)
        {
            anim.CrossFade("stay", 0f);
            anim.SetInteger("state", 0);
        }
        
        isInvincible = true;
        invincibleTimer = invincibleDuration;
        isBlinking = true;
        blinkTimer = blinkInterval;
        sprite.enabled = true;
    }

    private bool IsGrounded()
    {
        Vector2 boxSize = new Vector2(coll.bounds.size.x * 0.9f, coll.bounds.size.y * 0.1f);
        Vector2 castOrigin = new Vector2(coll.bounds.center.x, coll.bounds.center.y - coll.bounds.size.y / 2 + 0.05f);
        return Physics2D.BoxCast(castOrigin, boxSize, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (isPlayingDeathAnimation)
        {
            anim.SetInteger("state", 7);
            return;
        }
        
        if (isHurt)
        {
            state = MovementState.hit;
        }
        else if (isAttacking)
        {
            state = MovementState.hit;
        }
        else if (isDefending)
        {
            state = MovementState.def;
        }
        else if (isCasting)
        {
            state = MovementState.cast;
        }
        else if (rb.linearVelocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        else if (rb.linearVelocity.y < -0.1f)
        {
            state = MovementState.fall;
        }
        else if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.stay;
        }

        anim.SetInteger("state", (int)state);
    }
}
