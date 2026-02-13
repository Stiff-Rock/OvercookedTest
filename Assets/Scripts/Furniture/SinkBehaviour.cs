using UnityEngine;

// TODO: SinkBehaviour
[RequireComponent(typeof(ProgressSliderBehaviour))]
public class SinkBehaviour : InteractiveAppliance
{
    private ProgressSliderBehaviour progressBar;

    private void Awake()
    {
        progressBar = GetComponent<ProgressSliderBehaviour>();
    }
}
