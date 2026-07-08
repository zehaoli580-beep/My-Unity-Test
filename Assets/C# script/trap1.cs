using UnityEngine;

public class Trap1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("陷阱触发碰撞！对象: " + other.name + ", Tag: " + other.tag);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("检测到玩家！准备调用TakeDamage()");
            player playerScript = other.GetComponent<player>();
            if (playerScript != null)
            {
                Debug.Log("成功获取player组件！");
                playerScript.TakeDamage();
            }
            else
            {
                Debug.LogError("未找到player组件！");
            }
        }
    }
}
