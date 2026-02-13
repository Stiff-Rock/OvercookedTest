using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    // GameOverPanel
    [SerializeField] private TextMeshProUGUI deliveredOrdersText;
    [SerializeField] private TextMeshProUGUI failedOrdersText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Values")]
    [SerializeField] private int currentScore;
    private int deliveredOrdersCount;
    private int failedOrdersCount;

    [Header("Score Settings")]
    [SerializeField] private int ingredientScoreValue = 50;
    [SerializeField] private int timeScoreValue = 5;
    [SerializeField] private int expireScorePenalty = 100;

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

    public void UpdateScore(KitchenOrder order)
    {
        if (order.IsCompleted())
        {
            int orderScore = order.Recipe.GetTotalIngredients() * ingredientScoreValue;
            int lifespanScoreBonus = (int)order.Lifespan * timeScoreValue;
            int totalScore = orderScore + lifespanScoreBonus;
            UpdateScoreValue(totalScore);
            ++deliveredOrdersCount;
        }
        else
        {
            PenalizeScore();
            ++failedOrdersCount;
        }
    }

    public void PenalizeScore()
    {
        UpdateScoreValue(-expireScorePenalty);
    }

    private void UpdateScoreValue(int change)
    {
        currentScore += change;
        scoreText.SetText($"{currentScore}");
    }

    public void ShowFinalScore()
    {
        deliveredOrdersText.SetText($"Orders Delivered: {deliveredOrdersCount}");
        failedOrdersText.SetText($"Orders Failed: {failedOrdersCount}");
        finalScoreText.SetText($"Final Score: {currentScore}");
    }
}
