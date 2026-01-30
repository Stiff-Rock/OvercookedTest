using UnityEngine;

[RequireComponent(typeof(ProgressSliderBehaviour))]
public class StoveBehaviour : InteractiveAppliance
{
    private ProgressSliderBehaviour progressBar;

    [SerializeField] private float cookingPower;

    private void Awake()
    {
        progressBar = GetComponent<ProgressSliderBehaviour>();
        enabled = placedIngredient;
    }

    private void Update()
    {
        if (!placedIngredient) return;

        if (placedIngredient.IsBurnt())
        {
            ToggleActiveStove(false);
        }
        else
        {
            placedIngredient.Cook(Time.deltaTime);
            progressBar.UpdateProgressBar(placedIngredient);
        }
    }

    protected override void OnPlacedItemChanged()
    {
        bool isCooking = placedIngredient && !placedIngredient.IsBurnt();
        ToggleActiveStove(isCooking);
    }

    private void ToggleActiveStove(bool active)
    {
        enabled = active;
        progressBar.SetActive(active);
    }
}