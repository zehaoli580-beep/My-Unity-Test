using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("火球设置")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask groundLayer;
    
    private Vector2 moveDirection;
    private float lifetimeTimer;
    
    private void Start()
    {
        lifetimeTimer = lifetime;
    }
    
    private void Update()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
    
    public void Initialize(Vector2 direction)
    {
        moveDirection = direction;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = 1 << other.gameObject.layer;
        
        if ((otherLayer & enemyLayer) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
            Destroy(gameObject);
        }
        else if ((otherLayer & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
