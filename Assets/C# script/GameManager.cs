using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int maxLives = 3;
    public int currentLives;
    public int score = 0;
    public int failureSceneIndex = 2;
    public int fruitScore = 100;

    private Vector3 playerStartPosition;
    private HashSet<string> collectedFruits = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerStartPosition(Vector3 position)
    {
        playerStartPosition = position;
    }

    public Vector3 GetPlayerStartPosition()
    {
        return playerStartPosition;
    }

    public void PlayerDied()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            SceneManager.LoadScene(failureSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddScore()
    {
        score += fruitScore;
    }

    public bool IsFruitCollected(string fruitId)
    {
        return collectedFruits.Contains(fruitId);
    }

    public void MarkFruitCollected(string fruitId)
    {
        collectedFruits.Add(fruitId);
    }

    public void ResetGame()
    {
        currentLives = maxLives;
        score = 0;
        collectedFruits.Clear();
    }
}
