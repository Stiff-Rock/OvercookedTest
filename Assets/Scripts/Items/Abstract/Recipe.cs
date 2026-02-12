using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Recipe
{
    [Header("Type")]
    [SerializeField] public DishType DishType { get; private set; }

    [Header("Ingredients")]
    [SerializeField] private List<IngredientData> baseIngredients;
    [SerializeField] private List<IngredientData> extraIngredients;

    // Cache
    [SerializeField] private List<DishType> possibleRecipes;
    private RecipeScriptableObject matchedRecipe;

    public Recipe()
    {
        DishType = DishType.Undetermined;
        baseIngredients = new();
        extraIngredients = new();
    }

    public Recipe(DishType dishType, IngredientData[] baseIngredients)
    {
        this.DishType = dishType;
        this.baseIngredients = new(baseIngredients);
        extraIngredients = new();
    }

    // BUG: THE RECIPES CURRENTLY DONT DISTINGUISH FROM CUT, COOKED OR BURNT
    public bool TryAddIngredient(IngredientBehaviour newIngredient)
    {
        if (AlreadyContainsIngredient(newIngredient.Type)) return false;

        // If the recipe is already matched and finished, check for the extra ingredients
        if (RecipeIsFinished())
        {
            bool compatibleExtra = matchedRecipe.ExtraIngredients.Any(i => i.Type == newIngredient.Type);

            if (compatibleExtra)
                extraIngredients.Add(newIngredient.ToIngredientData());

            return compatibleExtra;
        }

        possibleRecipes ??= new(RecipesManager.Instance.DishToRecipe.Keys);

        // Check if the new ingredient is present in any of the possible recipes
        bool ingredientAccepted = possibleRecipes.Any(dish =>
        {
            RecipeScriptableObject recipe = RecipesManager.Instance.DishToRecipe[dish];
            return recipe.RequiredIngredients.Any(i => i.Type == newIngredient.Type);
        });

        if (ingredientAccepted)
        {
            baseIngredients.Add(newIngredient.ToIngredientData());
            FilterPossibleRecipes();
        }

        return ingredientAccepted;
    }

    public bool Matches(Recipe recipe)
    {
        if (recipe.DishType != DishType) return false;

        bool baseMatch = new HashSet<IngredientData>(baseIngredients)
                         .SetEquals(recipe.baseIngredients);

        bool extraMatch = new HashSet<IngredientData>(extraIngredients)
                          .SetEquals(recipe.extraIngredients);

        return baseMatch && extraMatch;
    }

    #region Helper Methods

    private bool AlreadyContainsIngredient(IngredientType newIngredient)
    {
        return baseIngredients.Any(i => i.Type == newIngredient) || extraIngredients.Any(i => i.Type == newIngredient);
    }

    // Filter recipes based on the current base ingredients
    private void FilterPossibleRecipes()
    {
        possibleRecipes.RemoveAll(dish => !IsCompatibleDish(dish));

        if (possibleRecipes.Count == 1)
        {
            DishType = possibleRecipes.First();
            matchedRecipe = RecipesManager.Instance.DishToRecipe[DishType];
        }
    }

    // Helper method to check a dish is compatible with the current base ingredients
    private bool IsCompatibleDish(DishType dish)
    {
        RecipeScriptableObject recipe = RecipesManager.Instance.DishToRecipe[dish];

        if (recipe == null)
        {
            Debug.LogError($"Recipe for dish {dish} not found in the dictionary.");
            return false;
        }

        return baseIngredients.All(i => recipe.RequiredIngredients.Contains(i));
    }

    private bool RecipeIsFinished()
    {
        return matchedRecipe && matchedRecipe.RequiredIngredients.Count() == baseIngredients.Count;
    }

    public bool TryAddExtra(IngredientData newExtra)
    {
        if (extraIngredients.Contains(newExtra))
            return false;

        extraIngredients.Add(newExtra);

        return true;
    }

    public int GetTotalIngredients()
    {
        return baseIngredients.Count + extraIngredients.Count;
    }

    #endregion

    public override string ToString()
    {
        return $"DishType: {DishType}" +
            $" | Base Ingredients {string.Join(", ", baseIngredients)}" +
            $" | Extra Ingredients {string.Join(", ", extraIngredients)}";
    }
}
