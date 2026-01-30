using UnityEngine;

[RequireComponent(typeof(ProgressSliderBehaviour))]
public class CuttingBoardBehaviour : InteractiveAppliance
{
    private ProgressSliderBehaviour progressBar;

    private void Awake()
    {
        progressBar = GetComponent<ProgressSliderBehaviour>();
    }
}
