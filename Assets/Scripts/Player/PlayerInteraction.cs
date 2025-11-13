using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Key interactKey;
    [SerializeField] private Key pickDropKey;

    [SerializeField] private Color playerColor;

    [SerializeField] private GameObject hand;

    private InteractiveObject activeAppliance;
    private IngredientBehaviour nearbyIngredient;

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
        // Check Interact
        if (Keyboard.current[interactKey].wasPressedThisFrame && activeAppliance)
            activeAppliance.OnInteract();

        // Check Pick/Drop
        if (Keyboard.current[pickDropKey].wasPressedThisFrame)
            PickOrDrop();
    }

    private void PickOrDrop()
    {
        // Place ingredient on appliance
        if (activeAppliance && pickedIngredient && activeAppliance.CanReceive())
        {
            pickedIngredient.ToggleColliders(false);

            activeAppliance.PlaceIngredient(pickedIngredient);
            pickedIngredient = null;
        }
        // Take ingredient from appliance
        else if (activeAppliance && !pickedIngredient && !activeAppliance.CanReceive())
        {
            pickedIngredient.ToggleColliders(false);

            pickedIngredient = activeAppliance.TakeIngredient();
            pickedIngredient.gameObject.transform.SetParent(hand.transform);
            pickedIngredient.transform.position = hand.transform.position;
        }
        // Take nearby ingredient
        else if (nearbyIngredient && !pickedIngredient)
        {
            pickedIngredient.ToggleColliders(false);

            pickedIngredient = activeAppliance.TakeIngredient();
            pickedIngredient.gameObject.transform.SetParent(hand.transform);
            pickedIngredient.transform.position = hand.transform.position;
        }
        // Drop currently held ingredient
        else if (pickedIngredient)
        {
            pickedIngredient.ToggleColliders(true);

            pickedIngredient.gameObject.transform.SetParent(null);
            pickedIngredient = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Before checking the new focused object, unfocus any previous one
        if (activeAppliance) activeAppliance = null;

        // Check if the collided object is an CookingStationBehaviour
        activeAppliance = collider.gameObject.GetComponent<InteractiveObject>();
    }

    private void OnTriggerExit(Collider collider)
    {
        // If the exited collision object was the previous CookingStationBehaviour
        if (activeAppliance && collider.gameObject == activeAppliance.gameObject)
        {
            activeAppliance = null;
        }
    }
}
