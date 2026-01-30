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

    // State
    [Header("DEBUG")]
    [SerializeField] private InteractiveAppliance nearbyAppliance;
    [SerializeField] private PickableItemBehaviour nearbyItem;
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
        nearbyAppliance = null;
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

        if (hit && hitInfo.collider.attachedRigidbody != null)
        {
            GameObject hitObject = hitInfo.collider.attachedRigidbody.gameObject;

            // Check if its an appliance
            if (hitObject.TryGetComponent(out InteractiveAppliance appliance))
            {
                nearbyAppliance = appliance;
            }
            // Check if its a pickable item
            else if (hitObject.TryGetComponent(out PickableItemBehaviour item))
            {
                nearbyItem = item;
            }
        }
    }

    private void CheckInputs()
    {
        // Check Interact
        if (Keyboard.current[interactKey].wasPressedThisFrame && nearbyAppliance)
            nearbyAppliance.OnInteract();

        // Check Pick/Drop
        if (Keyboard.current[pickDropKey].wasPressedThisFrame)
            PickOrDrop();
    }

    private void PickOrDrop()
    {
        // Place item on appliance
        if (CanPlaceItemOntoAppliance())
        {
            nearbyAppliance.PlaceItem(pickedItem);
            pickedItem = null;
        }
        // Take item from appliance
        else if (CanTakeItemFromAppliance())
        {
            pickedItem = nearbyAppliance.TakeItem();
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
        return nearbyAppliance && pickedItem && nearbyAppliance.CanReceive();
    }

    private bool CanTakeItemFromAppliance()
    {
        return nearbyAppliance && !pickedItem && !nearbyAppliance.CanReceive();
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