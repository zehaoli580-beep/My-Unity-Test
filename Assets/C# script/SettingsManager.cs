using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("音量组件")]
    public Slider volumeSlider; // 音量滑条
    public Toggle muteToggle;   // 静音开关

    void Start()
    {
        volumeSlider.value = AudioListener.volume; // 初始化音量
        muteToggle.isOn = AudioListener.volume <= 0;

        volumeSlider.onValueChanged.AddListener(ChangeVolume); // 绑定音量变化事件
        muteToggle.onValueChanged.AddListener(SwitchMute);     // 绑定静音切换事件
    }

    // 调节音量
    void ChangeVolume(float val)
    {
        AudioListener.volume = val;
        muteToggle.isOn = val <= 0;
    }

    // 切换静音
    void SwitchMute(bool isClose)
    {
        if (isClose)
            AudioListener.volume = 0;
        else
            AudioListener.volume = volumeSlider.value;
    }

    // 返回主菜单
    public void BackMenu()
    {
        SceneManager.LoadScene("star");
    }
}