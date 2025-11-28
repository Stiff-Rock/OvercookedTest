using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]

[ExecuteInEditMode]
public class PickableItemBehaviour : MonoBehaviour
{
    [SerializeField] private Collider physicsCollider;
    [SerializeField] private Collider triggerCollider;
    private Rigidbody rb;

    [SerializeField] protected string itemName;

    protected void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        physicsCollider = GetComponentInChildren<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    //TODO: ALSO LOCK RIGIDBODY ROTATION AND POSITION WHEN PICKED AND PLACED
    public void ToggleColliders(bool isEnabled)
    {
        triggerCollider.enabled = isEnabled;
        physicsCollider.enabled = isEnabled;
        
    }

    public void OnTransformParentChanged()
    {
        bool newParentTransformExists = transform.parent != null;
        ToggleColliders(!newParentTransformExists);
        if (newParentTransformExists) UpdateTransform();
    }

    private void UpdateTransform()
    {
        transform.position = transform.parent.position;
        transform.localRotation = Quaternion.identity;
    }

    #region Getters and Setters

    public bool isIngredient()
    {
        return this is IngredientBehaviour;
    }

    public bool isUtensil()
    {
        return this is UtensilBehaviour;
    }

    #endregion
}
