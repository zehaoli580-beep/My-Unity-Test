using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// 游戏管理器 - 采用单例模式，负责全局状态管理
/// </summary>
public class GameManager : MonoBehaviour
{
    // 单例实例 - 确保全局只有一个 GameManager
    public static GameManager Instance { get; private set; }

    // 游戏参数配置
    [Header("游戏参数")]
    public int maxLives = 3;          // 最大生命值
    public int currentLives;          // 当前生命值
    public int score = 0;             // 当前分数
    public int failureSceneIndex = 2; // 失败场景索引
    public int fruitScore = 100;      // 每个果子的分数

    // 内部状态
    private Vector3 playerStartPosition;                 // 玩家出生位置
    private HashSet<string> collectedFruits = new HashSet<string>(); // 已收集的果子（使用 HashSet 实现高效去重）

    /// <summary>
    /// 单例初始化 - 在游戏启动时执行
    /// </summary>
    private void Awake()
    {
        // 如果还没有实例，就将当前对象设为单例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切换场景时不销毁此对象，保持数据持久化
        }
        else
        {
            Destroy(gameObject); // 如果已有实例，销毁重复的对象
        }
    }

    /// <summary>
    /// 设置玩家出生位置
    /// </summary>
    public void SetPlayerStartPosition(Vector3 position)
    {
        playerStartPosition = position;
    }

    /// <summary>
    /// 获取玩家出生位置
    /// </summary>
    public Vector3 GetPlayerStartPosition()
    {
        return playerStartPosition;
    }

    /// <summary>
    /// 玩家受伤时调用 - 处理生命减少
    /// </summary>
    public void PlayerDied()
    {
        currentLives--;
    }

    /// <summary>
    /// 检查玩家是否死亡（生命耗尽）
    /// </summary>
    public bool IsPlayerDead()
    {
        return currentLives <= 0;
    }

    /// <summary>
    /// 加载失败场景
    /// </summary>
    public void LoadFailureScene()
    {
        SceneManager.LoadScene(failureSceneIndex);
    }

    /// <summary>
    /// 重新加载当前关卡
    /// </summary>
    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 增加分数
    /// </summary>
    public void AddScore()
    {
        score += fruitScore;
    }

    /// <summary>
    /// 检查果子是否已收集（用于跨关卡去重）
    /// </summary>
    public bool IsFruitCollected(string fruitId)
    {
        return collectedFruits.Contains(fruitId);
    }

    /// <summary>
    /// 标记果子已收集
    /// </summary>
    public void MarkFruitCollected(string fruitId)
    {
        collectedFruits.Add(fruitId);
    }

    /// <summary>
    /// 重置游戏状态 - 开始新游戏时调用
    /// </summary>
    public void ResetGame()
    {
        currentLives = maxLives;
        score = 0;
        collectedFruits.Clear();
    }
}
