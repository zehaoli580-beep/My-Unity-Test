using UnityEngine;
using UnityEngine.SceneManagement;

public class flag : MonoBehaviour
{
    [SerializeField] private AudioSource victorySound;
    [SerializeField] private bool isLastLevel = false;
    [SerializeField] private int victorySceneIndex = 3;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            if (victorySound != null && victorySound.clip != null)
            {
                victorySound.Play();
            }

            if (isLastLevel)
            {
                Invoke("LoadVictoryScene", victorySound != null ? victorySound.clip.length : 0.5f);
            }
            else
            {
                Invoke("LoadNextLevel", victorySound != null ? victorySound.clip.length : 0.5f);
            }
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void LoadVictoryScene()
    {
        SceneManager.LoadScene(victorySceneIndex);
    }
}
