using UnityEngine;

// TODO: UtensilBehaviour (Pots, Plates, Pans)
public class UtensilBehaviour : PickableItemBehaviour
{
    [SerializeField] private UtensilType utensilType;
    [SerializeField] private Recipe currentRecipe;

    private float stackHeightPos = 0f;

    protected override void Awake()
    {
        base.Awake();
        currentRecipe = new Recipe();
    }

    // Make a list of ingredients that the utensil accepts
    // (for example, a Pan would be able to hold meat but a Pot could not,
    // a plate could hold basically everything to be able to create the recipes)
    // ScriptablesObjects para el recetario

    // En caso del plato, una vez haya almacenado un ingrediente, eso hará 
    // que solo se puedan coger/sumar otros ingredientes compatibles.

    // Las ollas y sartenes solo podran almacenar un ingrediente a la vez
    // y pueden vertir su contenido sobre un plato, a no ser que esté
    // a medio cocinar

    // Todos pueden vertir su contenido en la basura en cualquier momento.

    // Las ollas, platos y sartenes no se pueden meter en el microondas

    // Las ollas y sartenes se usan poneindolas sobre una Stove

    // QUIZAS DEBE SER CADA INGREDIENTE EL QUE SEPA SOBRE QUE UTENSILIOS SE PUEDE PONER

    public void EmptyUtensil()
    {
        currentRecipe = new Recipe();
        DeleteChildren();
    }

    public bool TryAddIngredient(IngredientBehaviour ingredientItem)
    {
        bool added = currentRecipe.TryAddIngredient(ingredientItem.IngredientType);

        if (added)
        {
            // TODO: En vez de esto, haz que tengan un transform que sea "TOP" para stackear mas fácil
            ingredientItem.gameObject.transform.SetParent(transform, false);

            Renderer meshRenderer = ingredientItem.GetComponentInChildren<Renderer>();
            float itemHeight = meshRenderer.bounds.size.y;

            ingredientItem.gameObject.transform.localPosition = new Vector3(
                0,
                stackHeightPos + (itemHeight / 2f),
                0
            );

            stackHeightPos += itemHeight;
        }

        return added;
    }

    #region Helper Methods

    private void DeleteChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion
}