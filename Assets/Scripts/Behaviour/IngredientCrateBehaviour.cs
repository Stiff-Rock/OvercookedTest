using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientCrate : InteractiveObject
{
    private Animation anim;

    [SerializeField] private IngredientBehaviour ingredient;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public override IngredientBehaviour TakeIngredient()
    {
        anim.Play();
        return Instantiate(ingredient.gameObject).GetComponent<IngredientBehaviour>();
    }
}
