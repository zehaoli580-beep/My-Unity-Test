using UnityEngine;

public class JumpUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞物体是否是玩家
        if (other.CompareTag("Player"))
        {
            AddLifeAndDestroy();
        }
    }

    private void AddLifeAndDestroy()
    {
        // 获取 GameManager 实例并修改生命值
        if (GameManager.Instance != null)
        {
            GameManager.Instance.maxLives += 1;
            GameManager.Instance.currentLives += 1;
            
            // 打印调试信息，方便在控制台确认
            Debug.Log($"生命值已增加！当前最大生命: {GameManager.Instance.maxLives}, 当前生命: {GameManager.Instance.currentLives}");
        }
        else
        {
            Debug.LogWarning("未找到 GameManager 实例！");
        }

        // 物体消失
        Destroy(gameObject);
    }
}
