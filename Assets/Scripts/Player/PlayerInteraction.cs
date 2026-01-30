using UnityEngine;
using UnityEngine.InputSystem;

// TODO: PRIORIZAR EL MUEBLE AL QUE ESTA MIRANDO AL HABER VARIOS EN EL AREA

[RequireComponent(typeof(MeshRenderer))]
public class PlayerInteraction : MonoBehaviour
{
    // Controls
    [Header("CONTROLS")]
    [SerializeField] private Key interactKey;
    [SerializeField] private Key pickDropKey;

    // Values
    [Header("VALUES")]
    [SerializeField] private Vector3 initeracionArea;
    [SerializeField] private float interactionRaycastHeight;
    [SerializeField] private Color playerColor;

    // References
    [Header("REFERENCES")]
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
        InteractionCast();
        CheckInputs();
    }

    private void InteractionCast()
    {
        if (activeAppliance) activeAppliance = null;
        if (nearbyItem) nearbyItem = null;

        Vector3 origin = transform.position;
        origin.y = interactionRaycastHeight;

        bool hit = Physics.BoxCast(
            origin,
            initeracionArea / 2,
            transform.forward,
            out RaycastHit hitInfo,
            transform.rotation,
            initeracionArea.magnitude
        );

        if (hit)
        {
            // Check if the collided object is an CookingStationBehaviour
            activeAppliance = hitInfo.collider.gameObject.GetComponent<InteractiveAppliance>();

            if (!activeAppliance)
                nearbyItem = hitInfo.collider.gameObject.GetComponent<PickableItemBehaviour>();
        }
    }

    private void CheckInputs()
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
        // Place item on appliance
        if (CanPlaceItemOntoAppliance())
        {
            activeAppliance.PlaceItem(pickedItem);
            pickedItem = null;
        }
        // Take item from appliance
        else if (CanTakeItemFromAppliance())
        {
            pickedItem = activeAppliance.TakeItem();
            pickedItem.gameObject.transform.SetParent(hand.transform);
        }
        // Take nearby item
        else if (CanTakeNearbyItem())
        {
            pickedItem = nearbyItem;
            nearbyItem = null;
            pickedItem.gameObject.transform.SetParent(hand.transform);
        }
        // Drop currently held item
        else if (CanDropHeldItem())
        {
            pickedItem.gameObject.transform.SetParent(null);
            pickedItem = null;
        }
    }

    #region Helper Methods

    private bool CanPlaceItemOntoAppliance()
    {
        return activeAppliance && pickedItem && activeAppliance.CanReceive();
    }

    private bool CanTakeItemFromAppliance()
    {
        return activeAppliance && !pickedItem && !activeAppliance.CanReceive();
    }

    private bool CanTakeNearbyItem()
    {
        return nearbyItem && !pickedItem;
    }

    private bool CanDropHeldItem()
    {
        return pickedItem;
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position;
        origin.y = interactionRaycastHeight;

        Gizmos.DrawWireCube(origin, initeracionArea);
    }

    #endregion
}
