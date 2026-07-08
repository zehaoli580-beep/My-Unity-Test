using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    [SerializeField] private Text livesText;
    [SerializeField] private Text scoreText;

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            if (livesText != null)
            {
                livesText.text = "生命: " + GameManager.Instance.currentLives;
            }
            if (scoreText != null)
            {
                scoreText.text = "分数: " + GameManager.Instance.score;
            }
        }
    }
}
