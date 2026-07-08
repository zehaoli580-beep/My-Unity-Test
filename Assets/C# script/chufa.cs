using UnityEngine;

public class chufa : MonoBehaviour
{
    [Header("触发设置")]
    [Tooltip("拖入需要被触发飞行的 DROP 物体")]
    [SerializeField] private DROP[] targetDrops;

    [SerializeField] private bool triggerOnce = true;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && triggerOnce) return;

        if (other.CompareTag("Player"))
        {
            if (targetDrops != null && targetDrops.Length > 0)
            {
                foreach (DROP drop in targetDrops)
                {
                    if (drop != null)
                    {
                        drop.TriggerFly();
                    }
                }
                hasTriggered = true;
            }
            else
            {
                Debug.LogWarning("chufa 脚本未关联任何 DROP 物体！");
            }
        }
    }
}
