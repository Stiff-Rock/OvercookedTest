using UnityEngine;
using UnityEngine.UI;

public class StoveBehaviour : InteractiveObject
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
        float fillAmount = placedIngredient.GetRequiredCookingTime() / placedIngredient.GetCookedTime();
        progressBarSlider.fillAmount = fillAmount;
    }
}