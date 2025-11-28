using UnityEngine;

public class TrashBehaviour : InteractiveAppliance
{
    private Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public override void PlaceItem(PickableItemBehaviour dropped)
    {
        anim.Play();
        Destroy(dropped.gameObject);
    }
}
