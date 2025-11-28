using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientCrate : InteractiveAppliance
{
    private Animation anim;

    [SerializeField] private PickableItemBehaviour ingredient;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public override PickableItemBehaviour TakeItem()
    {
        anim.Play();
        return Instantiate(ingredient.gameObject).GetComponent<PickableItemBehaviour>();
    }
}
