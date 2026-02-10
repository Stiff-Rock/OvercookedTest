using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Game Settings")]
    [field: SerializeField] public RecipeScriptableObject[] Recipes { get; private set; }

    public Dictionary<DishType, RecipeScriptableObject> RecipesDict { get; private set; }

    [SerializeField] private int ingredientScoreValue = 50;
    [SerializeField] private int timeScoreValue = 25;
    [SerializeField] private int scorePenalty = 100;

    [Header("Game State")]
    [SerializeField] private List<Order> kitchenOrders;

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

        kitchenOrders = new();
        RecipesDict = new();
        foreach (var recipe in Recipes)
        {
            RecipesDict[recipe.dishType] = recipe;
        }
    }

    private void Start()
    {
        CreateOrder();
    }

    private void CreateOrder()
    {
        int randomIndex = Random.Range(0, Recipes.Length);
        RecipeScriptableObject selectedRecipe = Recipes[randomIndex];

        DishType type = selectedRecipe.dishType;
        IngredientType[] baseIngredients = selectedRecipe.requiredIngredients;
        IngredientType[] extraIngredients = selectedRecipe.extraIngredients;

        // Select extra ingredients
        int extrasAmount = Random.Range(0, extraIngredients.Length);

        Recipe orderRecipe = new(type, baseIngredients);
        for (int i = 0; i < extrasAmount; i++)

        {
            int newExtraIndex = Random.Range(0, extraIngredients.Length);
            IngredientType newExtra = extraIngredients[newExtraIndex];

            if (!orderRecipe.TryAddExtra(newExtra))
                i--;
        }

        int pointValue = (baseIngredients.Length + extraIngredients.Length) * ingredientScoreValue;

        kitchenOrders.Add(new(orderRecipe, pointValue));
    }

    public void ServeOrder(Recipe recipe)
    {
        foreach (Order order in kitchenOrders)
        {
            if (order.Recipe.Matches(recipe))
            {
                // add time score * timeScoreValue
                ScoreManager.Instance.UpdateScore(order);
                RemoveOrder(order);
                return;
            }
        }

        ScoreManager.Instance.UpdateScore(-scorePenalty);
        RemoveOrder(0);
    }

    private void RemoveOrder(Order orderToDelete)
    {
        RemoveOrder(kitchenOrders.IndexOf(orderToDelete));
    }

    private void RemoveOrder(int orderIndex)
    {
        kitchenOrders.RemoveAt(orderIndex);
        // Borrar UI también
    }
}
