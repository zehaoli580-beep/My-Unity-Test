using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead = false;

    [SerializeField] private AudioSource deathSound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetPlayerStartPosition(transform.position);
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        if (deathSound != null && deathSound.clip != null)
        {
            deathSound.Play();
        }

        anim.Play("death");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        player playerMovement = GetComponent<player>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
