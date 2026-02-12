using System.Collections.Generic;
using UnityEngine;

public class KitchenOrdersManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orderRowTransform;
    [SerializeField] private GameObject kitchenOrderPanelPrefab;

    [Header("Settings")]
    [SerializeField] private int maxOrders = 5;
    [SerializeField] private int maxIngredientsLimit = 4;
    [SerializeField] private float orderLifespan = 180.0f;

    [Header("Game State")]
    [SerializeField] private List<KitchenOrder> kitchenOrders;
    private List<GameObject> kitchenOrderPanels;

    private void Awake()
    {
        kitchenOrders = new();
        kitchenOrderPanels = new();
    }

    public void CreateOrder()
    {
        if (kitchenOrders.Count >= maxOrders) return;

        // Select a random recipe
        int randomIndex = Random.Range(0, RecipesManager.Instance.Recipes.Length);
        RecipeScriptableObject selectedRecipe = RecipesManager.Instance.Recipes[randomIndex];

        // Extract recipe data
        DishType type = selectedRecipe.DishType;
        IngredientData[] baseIngredients = selectedRecipe.RequiredIngredients;
        List<IngredientData> possibleExtraIngredients = new(selectedRecipe.ExtraIngredients);

        // Select a random extra ingredients amount
        int limit = Mathf.Min(possibleExtraIngredients.Count, maxIngredientsLimit - baseIngredients.Length);
        int extrasAmount = Random.Range(0, limit + 1);

        Recipe newOrderRecipe = new(type, baseIngredients);
        for (int i = 0; i < extrasAmount; i++)
        {
            if (possibleExtraIngredients.Count <= 0) break;

            int newExtraIndex = Random.Range(0, possibleExtraIngredients.Count);
            IngredientData newExtra = possibleExtraIngredients[newExtraIndex];

            if (!newOrderRecipe.TryAddExtra(newExtra))
                i--;

            possibleExtraIngredients.RemoveAt(newExtraIndex);
        }

        // Create the KitchenOrder
        GameObject newOrderObj = Instantiate(kitchenOrderPanelPrefab, orderRowTransform);
        kitchenOrderPanels.Add(newOrderObj);

        // Initialize the KitchenOrder data and listeners
        KitchenOrder newOrder = newOrderObj.GetComponent<KitchenOrder>();
        newOrder.OnExpire.AddListener(ScoreManager.Instance.PenalizeScore);
        newOrder.OnExpire.AddListener(() => RemoveOrder(newOrder));

        newOrder.Initialize(newOrderRecipe, orderLifespan);

        // Add the order to the list
        kitchenOrders.Add(newOrder);
    }

    public void ServeDish(Recipe recipe)
    {
        foreach (KitchenOrder order in kitchenOrders)
        {
            // Check if the served dish matches any placed orders
            if (order.Recipe.Matches(recipe))
            {
                order.SetIsCompleted(true);
                ScoreManager.Instance.UpdateScore(order);
                RemoveOrder(order);
                return;
            }
        }

        // No order matched the served dish
        ScoreManager.Instance.PenalizeScore();
    }

    private void RemoveOrder(KitchenOrder orderToDelete)
    {
        int orderToDeleteIndex = kitchenOrders.IndexOf(orderToDelete);
        kitchenOrders.RemoveAt(orderToDeleteIndex);

        GameObject orderToDeletePanel = kitchenOrderPanels[orderToDeleteIndex];
        kitchenOrderPanels.Remove(orderToDeletePanel);
        Destroy(orderToDeletePanel);
    }
}
