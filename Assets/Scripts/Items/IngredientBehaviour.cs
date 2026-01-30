using UnityEngine;

public class IngredientBehaviour : PickableItemBehaviour
{
    private Renderer objRenderer;

    [SerializeField] private Color cookedColor;
    [SerializeField] private Color burntColor;

    [SerializeField] private float requiredCookingTime = 2.0f;

    [SerializeField] private float requiredBurnTime = 3.0f;

    [SerializeField] private float cookedTime = 0f;

    private bool isCooked;

    private bool isBurnt;

    protected override void Awake()
    {
        base.Awake();

        isCooked = false;
        isBurnt = false;
        objRenderer = GetComponentInChildren<Renderer>();
    }

    public void Cook(float cookTime)
    {
        if (isBurnt) return;

        cookedTime += cookTime;

        if (!isCooked && cookedTime >= requiredCookingTime && cookedTime < requiredBurnTime)
        {
            isCooked = true;
            objRenderer.material.color = cookedColor;
            cookedTime = 0;
        }
        else if (isCooked && cookedTime >= requiredBurnTime)
        {
            isBurnt = true;
            objRenderer.material.color = burntColor;
        }
    }

    #region Getters and Setters

    public float GetRemainingTime()
    {
        return IsCooked() ? GetRequiredBurntTime() : GetRequiredCookingTime();
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

    #endregion
}
