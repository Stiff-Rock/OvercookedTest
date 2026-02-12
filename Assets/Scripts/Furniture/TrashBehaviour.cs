using UnityEngine;

public class TrashBehaviour : InteractiveAppliance
{
    private Animator anim;
    private static readonly int TrashAnimationTriggerKey = Animator.StringToHash("Use");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // BUG: Al tirar el contenido de un plato, se quita el plato de la mano del jugador
    public override void PlaceItem(PickableItemBehaviour dropped)
    {
        anim.SetTrigger(TrashAnimationTriggerKey);

        if (dropped is UtensilBehaviour utensil)
            utensil.EmptyUtensil();
        else
            Destroy(dropped.gameObject);
    }
}
