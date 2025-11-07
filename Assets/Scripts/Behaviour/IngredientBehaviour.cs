using UnityEngine;

public class IngredientBehaviour : MonoBehaviour
{
    private Renderer objRenderer;

    [SerializeField] private string ingredientName;
    [SerializeField] private Color cookedColor;
    [SerializeField] private Color burntColor;

    [SerializeField] private float requiredCookingTime = 2.0f;

    [SerializeField] private float requiredBurnTime = 3.0f;

    [SerializeField] private float cookedTime = 0f;

    private bool isCooked;

    private bool isBurnt;

    private void Awake()
    {
        isCooked = false;
        isBurnt = false;
        objRenderer = GetComponentInChildren<Renderer>();
    }

    public override string ToString()
    {
        return ingredientName;
    }

    public void Cook(float cookTime)
    {
        if (isBurnt) return;

        cookedTime += cookTime;

        if (!isCooked && cookedTime >= requiredCookingTime && cookedTime < requiredBurnTime)
        {
            isCooked = true;
            objRenderer.material.color = cookedColor;
        }
        else if (isCooked && cookedTime >= requiredBurnTime)
        {
            isBurnt = true;
            objRenderer.material.color = burntColor;
        }
    }

    public float GetRequiredCookingTime()
    {
        return requiredCookingTime;
    }

    public float GetRequiredBurntTime()
    {
        return requiredBurnTime;
    }

    public float GetCookedTime()
    {
        return cookedTime;
    }

    public bool IsCooked()
    {
        return isCooked;
    }

    public bool IsBurnt()
    {
        return isBurnt;
    }
}
