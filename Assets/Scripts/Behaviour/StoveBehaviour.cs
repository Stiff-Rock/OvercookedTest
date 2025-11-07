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
        if (storedIngredient != null && !storedIngredient.IsBurnt)
        {
            storedIngredient.Cook(Time.deltaTime);

            if (storedIngredient.IsCooked && !storedIngredient.IsBurnt)
                UpdateProgressBar();
        }
    }

    private void UpdateProgressBar()
    {
        float fillAmount = storedIngredient.RequiredCookingTime / storedIngredient.CookedTime;
        progressBarSlider.fillAmount = fillAmount;
    }

}
