using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected GameObject placeArea;
    [SerializeField] protected IngredientBehaviour storedIngredient;

    public IngredientBehaviour TakeIngredient()
    {
        IngredientBehaviour pickedIngredient = storedIngredient;
        storedIngredient = null;
        return pickedIngredient;
    }

    public void StoreIngredient(IngredientBehaviour newIngredient)
    {
        // Store ingredient
        storedIngredient = newIngredient;

        // Make it a child and put it in the place position
        storedIngredient.gameObject.transform.SetParent(placeArea.transform);
        storedIngredient.transform.localPosition = placeArea.transform.position;
    }

    public virtual void OnInteract()
    {
        Debug.Log("PARENT INTERACT");
    }

    public bool CanStore()
    {
        return storedIngredient == null;
    }
}
