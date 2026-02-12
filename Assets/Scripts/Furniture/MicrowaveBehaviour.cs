using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ProgressSliderBehaviour))]
public class MicrowaveBehaviour : InteractiveAppliance
{
    // References
    private Animator animator;
    private ProgressSliderBehaviour progressBar;

    [Header("SFX")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip oven;
    [SerializeField] private AudioClip ding;

    public bool isCooking = false;

    private void Awake()
    {
        progressBar = GetComponent<ProgressSliderBehaviour>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isCooking && placedIngredient)
        {
            progressBar.UpdateProgressBar(placedIngredient.GetCookProgress());
        }
    }

    private IEnumerator Cook()
    {
        if (placedIngredient)
        {
            ToggleCookAnimation(true);

            if (!placedIngredient.IsCooked)
            {
                while (!placedIngredient.IsCooked)
                {
                    placedIngredient.Cook(Time.deltaTime);
                    yield return null;
                }
            }
            else if (!placedIngredient.IsBurnt)
            {
                while (!placedIngredient.IsBurnt)
                {
                    placedIngredient.Cook(Time.deltaTime);
                    yield return null;
                }
            }

            ToggleCookAnimation(false);
        }
    }

    private void ToggleCookAnimation(bool isCooking)
    {
        this.isCooking = isCooking;
        enabled = isCooking;

        progressBar.SetActive(isCooking);
        animator.SetBool("isCooking", isCooking);
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.PlayOneShot(isCooking ? oven : ding);
    }

    public override void OnInteract(PlayerController playerController)
    {
        base.OnInteract(playerController);

        if (!isCooking && placedIngredient && !placedIngredient.IsBurnt)
            StartCoroutine(Cook());
    }
}
