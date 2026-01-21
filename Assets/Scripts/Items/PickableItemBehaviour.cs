using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[ExecuteInEditMode]
public class PickableItemBehaviour : MonoBehaviour
{
    [SerializeField] private Collider physicsCollider;
    [SerializeField] private Collider triggerCollider;
    private Rigidbody rb;

    [SerializeField] protected string itemName;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ToggleColliders(transform.parent == null);
    }

    public void ToggleColliders(bool isEnabled)
    {
        if (triggerCollider) triggerCollider.enabled = isEnabled;
        if (physicsCollider) physicsCollider.enabled = isEnabled;

        if (isEnabled)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        else
        {
            rb.constraints =
                // Freeze Position
                RigidbodyConstraints.FreezePositionX
                | RigidbodyConstraints.FreezePositionY
                | RigidbodyConstraints.FreezePositionZ
                // Freeze Rotation
                | RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationY
                | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void UpdateTransform()
    {
        transform.position = transform.parent.position;
        transform.localRotation = Quaternion.identity;
    }

    public void OnTransformParentChanged()
    {
        bool newParentTransformExists = transform.parent != null;
        ToggleColliders(!newParentTransformExists);
        if (newParentTransformExists) UpdateTransform();
    }

    #region Getters and Setters

    public bool IsIngredient()
    {
        return this is IngredientBehaviour;
    }

    public bool IsUtensil()
    {
        return this is UtensilBehaviour;
    }

    #endregion
}
