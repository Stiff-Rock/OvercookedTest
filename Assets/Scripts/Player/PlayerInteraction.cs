using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshRenderer))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Key interactKey;
    [SerializeField] private Key pickDropKey;

    [SerializeField] private Color playerColor;

    [SerializeField] private GameObject hand;

    private InteractiveAppliance activeAppliance;
    private PickableItemBehaviour nearbyItem;

    [SerializeField] private PickableItemBehaviour pickedItem;

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
        {
            activeAppliance.OnInteract();
        }

        // Check Pick/Drop
        if (Keyboard.current[pickDropKey].wasPressedThisFrame)
        {
            PickOrDrop();
        }
    }

    // TODO: REFACTOR THIS IS ALL BROKEN 
    // BUG: REFACTOR THIS IS ALL BROKEN 
    private void PickOrDrop()
    {
        // Place item on appliance
        if (activeAppliance && pickedItem && activeAppliance.CanReceive())
        {
            activeAppliance.PlaceItem(pickedItem);
            pickedItem = null;
        }
        // Take item from appliance
        else if (activeAppliance && !pickedItem && !activeAppliance.CanReceive())
        {
            pickedItem = activeAppliance.TakeItem();
            pickedItem.gameObject.transform.SetParent(hand.transform);
        }
        // Take nearby item
        else if (nearbyItem && !pickedItem)
        {
            pickedItem = nearbyItem;
            nearbyItem = null;
            pickedItem.gameObject.transform.SetParent(hand.transform);
        }
        // Drop currently held item
        else if (pickedItem)
        {
            pickedItem.gameObject.transform.SetParent(null);
            pickedItem = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Before checking the new focused object, unfocus any previous ones
        if (activeAppliance) activeAppliance = null;
        if (nearbyItem) nearbyItem = null;

        // Check if the collided object is an CookingStationBehaviour
        activeAppliance = collider.gameObject.GetComponent<InteractiveAppliance>();

        if (!activeAppliance)
            nearbyItem = collider.gameObject.GetComponent<PickableItemBehaviour>();
    }

    private void OnTriggerExit(Collider collider)
    {
        // If the exited collision object was the previous InteractiveAppliance
        if (activeAppliance && collider.gameObject == activeAppliance.gameObject)
        {
            activeAppliance = null;
        }

        // If the exited collision object was the previous PickableItemBehaviour
        if (nearbyItem && collider.gameObject == nearbyItem.gameObject)
        {
            nearbyItem = null;
        }
    }
}
