using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [Header("UI设置")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay;
    
    [Header("颜色设置")]
    [SerializeField] private Color readyColor = Color.white;
    [SerializeField] private Color cooldownColor = Color.gray;
    
    private float cooldownTime = 0f;
    private float currentCooldown = 0f;
    private bool isOnCooldown = false;

    private void Start()
    {
        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 0f;
        }
        if (iconImage != null)
        {
            iconImage.color = readyColor;
        }
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            currentCooldown -= Time.deltaTime;
            
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = currentCooldown / cooldownTime;
            }
            
            if (currentCooldown <= 0f)
            {
                isOnCooldown = false;
                currentCooldown = 0f;
                if (cooldownOverlay != null)
                {
                    cooldownOverlay.fillAmount = 0f;
                }
                if (iconImage != null)
                {
                    iconImage.color = readyColor;
                }
            }
        }
    }

    public void StartCooldown(float duration)
    {
        cooldownTime = duration;
        isOnCooldown = true;
        currentCooldown = duration;
        
        if (iconImage != null)
        {
            iconImage.color = cooldownColor;
        }
    }

    public bool IsReady()
    {
        return !isOnCooldown;
    }
}
