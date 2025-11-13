using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected GameObject placeArea;
    [SerializeField] protected IngredientBehaviour placedIngredient;

    public virtual IngredientBehaviour TakeIngredient()
    {
        IngredientBehaviour pickedIngredient = placedIngredient;
        placedIngredient = null;
        return pickedIngredient;
    }

    public virtual void PlaceIngredient(IngredientBehaviour newIngredient)
    {
        // Store ingredient
        placedIngredient = newIngredient;

        // Make it a child and put it in the place position
        placedIngredient.gameObject.transform.SetParent(placeArea.transform);
        placedIngredient.transform.localPosition = placeArea.transform.position;
    }

    public virtual void OnInteract()
    {
    }

    public bool CanReceive()
    {
        return placedIngredient == null;
    }
}
