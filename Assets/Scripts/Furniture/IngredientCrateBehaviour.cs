using UnityEngine;

public class IngredientCrate : InteractiveAppliance
{
    private Animation anim;

    [SerializeField] private PickableItemBehaviour ingredient;

    private void Awake()
    {
        anim = GetComponentInChildren<Animation>();
    }

    public override PickableItemBehaviour TakeItem()
    {
        anim.Play();
        return Instantiate(ingredient.gameObject).GetComponent<PickableItemBehaviour>();
    }

    public override bool HasItem()
    {
        return true;
    }
}
