using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class MicrowaveBehaviour : InteractiveObject
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

    private void Start()
    {
        progressBarCanvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        if (isCooking && storedIngredient != null)
        {
            float required = storedIngredient.IsCooked() ? storedIngredient.GetRequiredBurntTime() : storedIngredient.GetRequiredCookingTime();
            float fillAmount = storedIngredient.GetCookedTime() / required;
            Debug.Log(storedIngredient.GetCookedTime() + " - " + storedIngredient.GetRequiredCookingTime() + " - " + fillAmount);
            progressBarSlider.fillAmount = fillAmount;
        }
    }

    private IEnumerator Cook()
    {
        SetCookAnimation(true);

        if (!storedIngredient.IsCooked())
            while (!storedIngredient.IsCooked())
            {
                storedIngredient.Cook(Time.deltaTime);
                yield return null;
            }
        else if (!storedIngredient.IsBurnt())
            while (!storedIngredient.IsBurnt())
            {
                storedIngredient.Cook(Time.deltaTime);
                yield return null;
            }

        SetCookAnimation(false);
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
        if (storedIngredient != null && !storedIngredient.IsBurnt())
            StartCoroutine(Cook());
    }
}
