using UnityEngine;

public class IngredientBehaviour : MonoBehaviour
{
    private Renderer objRenderer;

    [SerializeField] private string ingredientName;
    [SerializeField] private Color cookedColor;
    [SerializeField] private Color burntColor;

    [SerializeField] private float cookingTime = 2;

    private bool isCooked;
    private bool isBurnt;

    private void Awake()
    {
        isCooked = false;
        objRenderer = GetComponent<Renderer>();
    }

    public override string ToString()
    {
        return ingredientName;
    }

    public void Cook()
    {
        if (isBurnt) return;

        if (isCooked)
        {
            objRenderer.material.color = burntColor;
            isBurnt = true;
            return;
        }

        isCooked = true;
        objRenderer.material.color = cookedColor;
    }

    public bool IsBurnt()
    {
        return isBurnt;
    }

    public float GetCookingTime()
    {
        return cookingTime;
    }
}
