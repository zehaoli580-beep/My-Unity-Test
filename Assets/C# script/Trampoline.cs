using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("弹簧设置")]
    [Tooltip("玩家向上弹起的距离（N）")]
    [SerializeField] private float jumpHeight = 5f;
    
    [Tooltip("Animator 中触发弹簧动画的 Trigger 参数名称")]
    [SerializeField] private string animationTrigger = "Jump";

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            // 初始状态下停止动画器的运行，确保弹簧静止
            anim.speed = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 只有当 player 接触到弹簧时
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 确保是从顶端接触
                if (contact.normal.y < -0.5f)
                {
                    ActivateTrampoline(collision.gameObject);
                    break;
                }
            }
        }
    }

    private void ActivateTrampoline(GameObject playerObj)
    {
        Rigidbody2D rb = playerObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float launchVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, launchVelocity);

            if (anim != null)
            {
                // 恢复动画器速度并播放一次动画
                anim.speed = 1f;
                anim.Play(animationTrigger, 0, 0f);
                
                // 在下一帧或一段时间后，如果动画是非循环的，它会自动停在最后一帧
                // 或者我们可以通过协程在动画结束后将其速度再次设为 0
                StartCoroutine(StopAnimationAfterPlay());
            }
        }
    }

    private System.Collections.IEnumerator StopAnimationAfterPlay()
    {
        // 等待一小段时间让动画开始播放
        yield return new WaitForSeconds(0.1f);
        
        // 获取当前动画状态的长度
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        // 等待动画播放完毕
        yield return new WaitForSeconds(stateInfo.length);
        
        // 动画播放完后再次将速度设为 0，使其保持静止
        anim.speed = 0f;
    }
}
