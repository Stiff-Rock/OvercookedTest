using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

//TODO: DESHABILITAR UPDATE SI NO VA A COCINAR

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
            SetCookAnimation(true);

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

            SetCookAnimation(false);
        }
    }

    private void SetCookAnimation(bool isCooking)
    {
        this.isCooking = isCooking;

        progressBarCanvas.gameObject.SetActive(isCooking);
        animator.SetBool("isCooking", isCooking);
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.PlayOneShot(isCooking ? oven : ding);
    }

    public override void OnInteract()
    {
        if (placedIngredient && !placedIngredient.IsBurnt())
            StartCoroutine(Cook());
    }
}
