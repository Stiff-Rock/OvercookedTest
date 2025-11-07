using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Key interactKey;
    [SerializeField] private Key pickDropKey;

    [SerializeField] private Color playerColor;

    [SerializeField] private GameObject hand;

    private InteractiveObject activeAppliance;

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
            activeAppliance.OnInteract();

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
        else if (CanTake())
        {
            pickedIngredient = activeAppliance.TakeIngredient();
            pickedIngredient.gameObject.transform.SetParent(hand.transform);

            float offsetDistance = pickedIngredient.gameObject.transform.localScale.z / 1.5f;
            pickedIngredient.transform.position = hand.transform.position + hand.transform.forward * offsetDistance;
        }
    }

    private bool CanTake()
    {
        return pickedIngredient == null;

    }
    private void OnCollisionEnter(Collision collision)
    {
        // Before checking the new focused object, unfocus any previous one
        if (activeAppliance != null)
        {
            activeAppliance = null;
        }

        // Check if the collided object is an CookingStationBehaviour
        activeAppliance = collision.gameObject.GetComponent<InteractiveObject>();
    }

    private void OnCollisionExit(Collision collision)
    {
        // If the exited collision object was the previous CookingStationBehaviour
        if (activeAppliance != null && collision.gameObject == activeAppliance.gameObject)
        {
            activeAppliance = null;
        }
    }
}
