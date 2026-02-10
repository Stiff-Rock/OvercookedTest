using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int score;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void UpdateScore(int amount)
    {
        score += amount;
    }

    #region Getters and Setters

    public void SetScore(int newScore)
    {
        score = newScore;
    }

    public int GetScore()
    {
        return score;
    }

    #endregion
}
