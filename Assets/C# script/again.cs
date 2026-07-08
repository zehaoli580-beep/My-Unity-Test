using UnityEngine;
using UnityEngine.SceneManagement;

public class again : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
