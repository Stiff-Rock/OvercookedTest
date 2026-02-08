using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recipe
{
    [Header("Type")]
    private DishType dishType;

    [Header("Ingredients")]
    private readonly List<IngredientType> baseIngredients;
    private readonly List<IngredientType> extraIngredients;

    // Cache
    private List<DishType> possibleRecipes;
    private RecipeScriptableObject matchedRecipe;

    public Recipe()
    {
        dishType = DishType.None;
        baseIngredients = new();
        extraIngredients = new();
    }

    // Add methods to add ingredients, to compare two recipes, 
    // and to check if the new ingredient is compatible and what recipe it makes
    // also in that process keep stored the possible outcomes so no need to recalculate them each time
    public bool TryAddIngredient(IngredientType ingredient)
    {
        // If the recipe is matched, check for extras
        if (matchedRecipe)
        {
            if (extraIngredients.Contains(ingredient)) return false;

            bool compatibleExtra = matchedRecipe.extraIngredients.Contains(ingredient);

            if (compatibleExtra)
                extraIngredients.Add(ingredient);

            return compatibleExtra;
        }

        // Unknown recipe
        // If it's the first ingredient accept it and filter possible recipes
        if (possibleRecipes == null)
        {
            possibleRecipes = new(GameController.Instance.RecipesDict.Keys);
            baseIngredients.Add(ingredient);
        }
        // Check if the new ingredient is compatible with any of the possible recipes
        else
        {

        }

        FilterPossibleRecipes();

        return false; // TODO: BORRAR
    }

    // Filter recipes based on the current base ingredients
    private void FilterPossibleRecipes()
    {
        possibleRecipes.RemoveAll(dish => !IsCompatibleDish(dish));

        if (possibleRecipes.Count == 1)
        {
            dishType = possibleRecipes.First();
            matchedRecipe = GameController.Instance.RecipesDict[dishType];
        }
    }

    // Helper method to check a dish is compatible with the current base ingredients
    private bool IsCompatibleDish(DishType dish)
    {
        RecipeScriptableObject recipe = GameController.Instance.RecipesDict[dish];

        if (recipe == null)
        {
            Debug.LogError($"Recipe for dish {dish} not found in the dictionary.");
            return false;
        }

        return baseIngredients.All(i => recipe.requiredIngredients.Contains(i));
    }

    /* public bool Matches(RecipeScriptableObject recipe)
     {
         return recipe.dishType == dishType
             && recipe.baseIngredients.SetEquals(baseIngredients)
             && recipe.extraIngredients.SetEquals(extraIngredients);
     }*/
}
