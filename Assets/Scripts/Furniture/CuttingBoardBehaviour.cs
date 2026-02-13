using UnityEngine;

[RequireComponent(typeof(ProgressSliderBehaviour))]
public class CuttingBoardBehaviour : InteractiveAppliance
{
    // References
    private ProgressSliderBehaviour progressBar;

    // Flags
    private bool isCutting;

    private void Awake()
    {
        progressBar = GetComponent<ProgressSliderBehaviour>();
        enabled = false;
    }

    private void Update()
    {
        if (placedIngredient && isCutting)
        {
            progressBar.UpdateProgressBar(placedIngredient.GetCutProgress());
            placedIngredient.Cut(Time.deltaTime);

            if (placedIngredient.IsCut)
            {
                isCutting = false;
                progressBar.SetActive(false);
                enabled = false;

                currentPlayer.ToggleActive(true);
            }
        }
    }

    public override void OnInteract(PlayerController playerController)
    {
        base.OnInteract(playerController);

        if (placedIngredient)
        {
            isCutting = true;
            progressBar.SetActive(true);
            enabled = true;

            currentPlayer.ToggleActive(false);
        }
    }
}