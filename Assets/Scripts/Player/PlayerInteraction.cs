using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshRenderer))]
public class PlayerInteraction : MonoBehaviour
{
    // Controls
    [Header("CONTROLS")]
    [SerializeField] private Key interactKey;
    [SerializeField] private Key pickDropKey;

    // Values
    [Header("INTERACTION PHYSICS")]
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Vector2 interactionBox;
    private Vector3 halfExtents;
    [SerializeField] private float interactionRange;
    [SerializeField] private float heightOffset;
    private Vector3 yOffset;

    // Values
    [Header("VALUES")]
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
        yOffset = Vector3.up * heightOffset;
        halfExtents = interactionBox / 2;
    }

    private void Update()
    {
        InteractionCast();
        CheckInputs();
    }

    private void InteractionCast()
    {
        activeAppliance = null;
        nearbyItem = null;

        Vector3 center = transform.position + yOffset;

        bool hit = Physics.BoxCast(
            center,
            halfExtents,
            transform.forward,
            out RaycastHit hitInfo,
            transform.rotation,
            interactionRange,
            interactionLayer
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
        Vector3 localCenter = new(0, heightOffset, interactionRange / 2);
        Vector3 size = new(interactionBox.x, interactionBox.y, interactionRange);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(localCenter, size);
        Gizmos.matrix = Matrix4x4.identity;
    }

    #endregion
}