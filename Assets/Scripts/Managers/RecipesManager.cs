using System.Collections.Generic;
using UnityEngine;

public class RecipesManager : MonoBehaviour
{
    public static RecipesManager Instance { get; private set; }

    [field: SerializeField] public RecipeScriptableObject[] Recipes { get; private set; }
    public Dictionary<DishType, RecipeScriptableObject> DishToRecipe { get; private set; }

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
            return;
        }

    }

    private void Start()
    { 
        InitializeDishDictionary();
    }

    private void InitializeDishDictionary()
    {
        DishToRecipe = new();
        foreach (RecipeScriptableObject recipe in Recipes)
        {
            DishToRecipe[recipe.DishType] = recipe;
        }
    }
}
