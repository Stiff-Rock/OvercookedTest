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
        Vector3 placePosition = placeArea.transform.position;
        placePosition.y += storedIngredient.gameObject.transform.localScale.y / 4;
        storedIngredient.transform.localPosition = placePosition;
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
