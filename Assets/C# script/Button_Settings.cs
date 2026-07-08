using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Settings : MonoBehaviour
{
    // 打开设置场景
    public void OpenSettingsScene()
    {
        SceneManager.LoadScene("settings");
    }
}