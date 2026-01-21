using UnityEngine;

public class TrashBehaviour : InteractiveAppliance
{
    private Animator anim;
    private static readonly int TrashAnimationTriggerKey = Animator.StringToHash("Use");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public override void PlaceItem(PickableItemBehaviour dropped)
    {
        anim.SetTrigger(TrashAnimationTriggerKey);
        Destroy(dropped.gameObject);
    }
}
