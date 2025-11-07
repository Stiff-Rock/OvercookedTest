using UnityEngine;

public class IngredientBehaviour : MonoBehaviour
{
    private Renderer objRenderer;

    [SerializeField] private string ingredientName;
    [SerializeField] private Color cookedColor;
    [SerializeField] private Color burntColor;

    [SerializeField] private float requiredCookingTime = 2.0f;
    public float RequiredCookingTime { get; }

    [SerializeField] private float requiredBurnTime = 3.0f;
    public float RequiredBurnTime { get; }


    [SerializeField] private float cookedTime = 0f;
    public float CookedTime { get; }

    private bool isCooked;
    public bool IsCooked { get; }

    private bool isBurnt;
    public bool IsBurnt { get; }

    private void Awake()
    {
        isCooked = false;
        isBurnt = false;
        objRenderer = GetComponent<Renderer>();
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
            Debug.Log("INGREDIENT COOKED");
            isCooked = true;
            objRenderer.material.color = cookedColor;
        }
        else if (isCooked && cookedTime >= requiredBurnTime)
        {
            Debug.Log("INGREDIENT BURNT");
            isBurnt = true;
            objRenderer.material.color = burntColor;
        }
    }

    public float GetRemainingTime()
    {
        if (!isCooked)
            return requiredCookingTime - cookedTime;
        else if (!isBurnt)
            return requiredBurnTime - cookedTime;
        else
            return 0;
    }
}
