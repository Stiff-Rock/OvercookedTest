using UnityEngine;

// TODO: UtensilBehaviour (Pots, Plates, Pans)
public class UtensilBehaviour : PickableItemBehaviour
{
    [field: SerializeField] public UtensilType UtensilType { get; private set; }
    [field: SerializeField] public Recipe CurrentRecipe { get; private set; }

    [SerializeField] private Transform utensilContentTransform;

    private float stackHeightPos = 0f;

    protected override void Awake()
    {
        base.Awake();
        CurrentRecipe = new Recipe();
    }

    // Make a list of ingredients that the utensil accepts
    // (for example, a Pan would be able to hold meat but a Pot could not,
    // QUIZAS DEBE SER CADA INGREDIENTE EL QUE SEPA SOBRE QUE UTENSILIOS SE PUEDE PONER

    // Las ollas y sartenes se usan poneindolas sobre una Stove

    // Las ollas y sartenes solo podran almacenar un ingrediente a la vez
    // y pueden vertir su contenido sobre un plato, a no ser que esté
    // a medio cocinar

    // Las ollas, platos y sartenes no se pueden meter en el microondas

    public void EmptyUtensil()
    {
        CurrentRecipe = new Recipe();
        DeleteChildren();
    }

    public bool TryAddIngredient(IngredientBehaviour ingredientItem)
    {
        bool added = CurrentRecipe.TryAddIngredient(ingredientItem);

        if (!added) return false;

        if (UtensilType == UtensilType.Pan)
        {
        }
        else if (UtensilType == UtensilType.Plate)
        {
        }
        else if (UtensilType == UtensilType.Pot)
        {
        }
        else
        {
            Debug.LogError($"Utensil '{gameObject.name}' has no valid UtensilType (found {UtensilType})");
            return false;
        }

        StackIngredients(ingredientItem);

        return true;
    }

    #region Helper Methods

    private void DeleteChildren()
    {
        if (!utensilContentTransform)
        {
            Debug.LogWarning("Cannot DeleteChildren since utensilContentTransform is null");
            return;
        }

        foreach (Transform child in utensilContentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    // TODO: En vez de esto, haz que tengan un transform que sea "TOP" para stackear mas fácil
    private void StackIngredients(IngredientBehaviour ingredientItem)
    {
        if (!utensilContentTransform)
        {
            Debug.LogWarning("Cannot StackIngredients since utensilContentTransform is null");
            return;
        }

        ingredientItem.gameObject.transform.SetParent(utensilContentTransform, false);

        Renderer meshRenderer = ingredientItem.GetComponentInChildren<Renderer>();
        float itemHeight = meshRenderer.bounds.size.y;

        ingredientItem.gameObject.transform.localPosition = new Vector3(
                    0,
            stackHeightPos + (itemHeight / 2f),
                    0
        );

        stackHeightPos += itemHeight;
    }

    #endregion
}