using UnityEngine;

[RequireComponent(typeof(ProgressSliderBehaviour))]
public class StoveBehaviour : InteractiveAppliance
{
    private ProgressSliderBehaviour progressBar;

    [Header("Cooking Settings")]
    [Range(0.5f, 2f)]
    [SerializeField] private float cookingPower = 1.0f;

    private void Awake()
    {
        progressBar = GetComponent<ProgressSliderBehaviour>();
        enabled = placedIngredient;
    }

    private void Update()
    {
        if (!placedIngredient) return;

        if (IsFinishedCooking())
        {
            ToggleActiveStove(false);
        }
        else
        {
            placedIngredient.Cook(Time.deltaTime * cookingPower);
            progressBar.UpdateProgressBar(placedIngredient.GetCookProgress());
        }
    }

    protected override void OnPlacedItemChanged()
    {
        bool isCooking = placedIngredient && !placedIngredient.IsBurnt;
        ToggleActiveStove(isCooking);
    }

    private void ToggleActiveStove(bool active)
    {
        enabled = active;
        progressBar.SetActive(active);
    }

    private bool IsFinishedCooking()
    {
        return cookingPower > 0.7 && placedIngredient.IsBurnt || cookingPower <= 0.7 && placedIngredient.IsCooked;
    }
}