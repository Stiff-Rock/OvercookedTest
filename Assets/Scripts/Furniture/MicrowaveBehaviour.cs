using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class MicrowaveBehaviour : InteractiveAppliance
{
    private Animator animator;

    [Header("GUI")]
    [SerializeField] private Canvas progressBarCanvas;
    [SerializeField] private Image progressBarSlider;

    [Header("SFX")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip oven;
    [SerializeField] private AudioClip ding;

    public bool isCooking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
        progressBarCanvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        if (isCooking && placedIngredient)
        {
            float required = placedIngredient.IsCooked() ? placedIngredient.GetRequiredBurntTime() : placedIngredient.GetRequiredCookingTime();
            float fillAmount = placedIngredient.GetCookedTime() / required;
            progressBarSlider.fillAmount = fillAmount;
        }
    }

    // TODO: REPENSAR ESTO
    private IEnumerator Cook()
    {
        if (placedIngredient)
        {
            ToggleCookAnimation(true);

            if (!placedIngredient.IsCooked())
            {
                while (!placedIngredient.IsCooked())
                {
                    placedIngredient.Cook(Time.deltaTime);
                    yield return null;
                }
            }
            else if (!placedIngredient.IsBurnt())
            {
                while (!placedIngredient.IsBurnt())
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

        progressBarCanvas.gameObject.SetActive(isCooking);
        animator.SetBool("isCooking", isCooking);
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.PlayOneShot(isCooking ? oven : ding);
    }

    public override void OnInteract()
    {
        if (!isCooking && placedIngredient && !placedIngredient.IsBurnt()) { 
            StartCoroutine(Cook());
        }
    }
}
