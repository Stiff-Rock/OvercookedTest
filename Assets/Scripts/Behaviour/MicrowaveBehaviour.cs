using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
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
        if (storedIngredient != null)
        {
            float fillAmount = storedIngredient.RequiredCookingTime / storedIngredient.CookedTime;
            progressBarSlider.fillAmount = fillAmount;
        }
    }

    private IEnumerator Cook()
    {
        SetCookAnimation(true);

        //TODO: EN EL MICROONDAS NO SE QUEMA
        if (!storedIngredient.IsCooked)
            while (!storedIngredient.IsCooked)
            {
                storedIngredient.Cook(Time.deltaTime);
                yield return null;
            }
        else if (!storedIngredient.IsBurnt)
            while (!storedIngredient.IsBurnt)
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
        audioSource.Stop();
        audioSource.PlayOneShot(isCooking ? oven : ding);
    }

    public override void OnInteract()
    {
        if (storedIngredient != null && !storedIngredient.IsBurnt)
            StartCoroutine(Cook());
    }
}
