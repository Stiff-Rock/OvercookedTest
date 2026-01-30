using UnityEngine;
using UnityEngine.UI;

//TODO: DESHABILITAR UPDATE SI NO VA A COCINAR

public class StoveBehaviour : InteractiveAppliance
{
    [Header("GUI")]
    [SerializeField] private Canvas progressBarCanvas;
    [SerializeField] private Image progressBarSlider;

    [SerializeField] private float cookingPower;

    private void Update()
    {
        if (placedIngredient && !placedIngredient.IsBurnt())
        {
            placedIngredient.Cook(Time.deltaTime);

            if (placedIngredient.IsCooked() && !placedIngredient.IsBurnt())
                UpdateProgressBar();
        }
    }

    private void UpdateProgressBar()
    {
        if (placedIngredient)
        {
            float fillAmount = placedIngredient.GetRequiredCookingTime() / placedIngredient.GetCookedTime();
            progressBarSlider.fillAmount = fillAmount;
        }
    }

    public override void PlaceItem(PickableItemBehaviour newItem)
    {
        base.PlaceItem(newItem);
        enabled = placedIngredient;
    }
}