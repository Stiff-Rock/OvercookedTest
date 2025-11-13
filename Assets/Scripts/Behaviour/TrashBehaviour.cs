using UnityEngine;

public class TrashBehaviour : InteractiveObject
{
    private Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public override void PlaceIngredient(IngredientBehaviour dropped)
    {
        anim.Play();
        Destroy(dropped.gameObject);
    }
}
