using UnityEngine;

// BUG: THE RECIPES CURRENTLY DONT DISTINGUISH FROM CUT, COOKED OR BURNT
[CreateAssetMenu(fileName = "NewRecipe", menuName = "Kitchen/Recipe")]
public class RecipeScriptableObject : ScriptableObject
{
    [Header("Type")]
    [field: SerializeField] public DishType DishType { get; private set; }

    [Header("Ingredients")]
    [field: SerializeField] public IngredientType[] RequiredIngredients { get; private set; }
    [field: SerializeField] public IngredientType[] ExtraIngredients { get; private set; }

    [Header("Visual")]
    [field: SerializeField] public GameObject ResultPrefab { get; private set; }
}
