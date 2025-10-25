using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Key interactKey;
    [SerializeField] private Key pickDropKey;

    [SerializeField] private Color playerColor;

    [SerializeField] private GameObject hand;

    private InteractObject activeAppliance;

    [SerializeField] private IngredientBehaviour pickedIngredient;

    private void Awake()
    {
        GetComponent<MeshRenderer>().material.color = playerColor;
    }

    private void Update()
    {
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        if (activeAppliance == null) return;

        // Check Interact
        if (Keyboard.current[interactKey].wasPressedThisFrame)
            activeAppliance.Interact();

        // Check Pick/Drop
        if (Keyboard.current[pickDropKey].wasPressedThisFrame)
            PickOrDrop();
    }

    private void PickOrDrop()
    {
        if (activeAppliance.CanStore())
        {
            activeAppliance.StoreIngredient(pickedIngredient);
            pickedIngredient = null;
        }
        else if (HasEmptyHands() && !activeAppliance.IsCooking())
        {
            pickedIngredient = activeAppliance.GiveIngredient();
            pickedIngredient.gameObject.transform.SetParent(hand.transform);

            float offsetDistance = pickedIngredient.gameObject.transform.localScale.z / 1.5f;
            pickedIngredient.transform.position = hand.transform.position + hand.transform.forward * offsetDistance;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Before checking the new focused object, unfocus any previous one
        if (activeAppliance != null)
        {
            activeAppliance.Unfocus();
            activeAppliance = null;
        }

        // Check if the collided object is an InteractObject
        activeAppliance = collision.gameObject.GetComponent<InteractObject>();

        // If its an InteractObject, focus on it
        if (activeAppliance != null) activeAppliance.Focus();
    }

    private void OnCollisionExit(Collision collision)
    {
        // If the exited collision object was the previous InteractObject, unfocus
        if (activeAppliance != null && collision.gameObject == activeAppliance.gameObject)
        {
            activeAppliance.Unfocus();
            activeAppliance = null;
        }
    }

    private bool HasEmptyHands()
    {
        return pickedIngredient == null;
    }
}
