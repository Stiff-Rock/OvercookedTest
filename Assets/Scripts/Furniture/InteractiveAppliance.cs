using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

//TODO: CAMBIAR A COMPONENTES EN VEZ DE HERENCIA
public class InteractiveAppliance : MonoBehaviour
{
    [SerializeField] protected string applianceName;
    [SerializeField] protected GameObject placeArea;
    [SerializeField] private PickableItemBehaviour _placedItem;

    protected PickableItemBehaviour PlacedItem
    {
        get { return _placedItem; }
        set
        {
            if (_placedItem == value) return;

            _placedItem = value;

            if (_placedItem)
            {
                if (_placedItem.isIngredient())
                {
                    placedIngredient = _placedItem.GetComponent<IngredientBehaviour>();
                    placedUtensil = null;
                }
                else if (_placedItem.isUtensil())
                {
                    placedUtensil = _placedItem.GetComponent<UtensilBehaviour>();
                    placedIngredient = null;
                }
            }
            else
            {
                placedIngredient = null;
                placedUtensil = null;
            }
        }
    }

    protected IngredientBehaviour placedIngredient;
    protected UtensilBehaviour placedUtensil;

    protected virtual void Start()
    {
        PlacedItem = placeArea.GetComponentInChildren<PickableItemBehaviour>();
    }

    public virtual PickableItemBehaviour TakeItem()
    {
        PickableItemBehaviour pickedItem = PlacedItem;
        PlacedItem = null;
        return pickedItem;
    }

    public virtual void PlaceItem(PickableItemBehaviour newItem)
    {
        // Store item
        PlacedItem = newItem;

        // Make it a child and put it in the place position
        PlacedItem.gameObject.transform.SetParent(placeArea.transform);
    }

    // Virtual method to be overridden by child classes
    public virtual void OnInteract()
    {
    }

    public bool CanReceive()
    {
        return PlacedItem == null;
    }
}
