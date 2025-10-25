using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractObject : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Animator animator;

    [SerializeField] private Canvas progressBarCanvas;
    [SerializeField] private Image progressBarSlider;

    private AudioSource audioSource;
    [SerializeField] private AudioClip oven;
    [SerializeField] private AudioClip ding;

    [SerializeField] private Color focusColor = Color.yellow;
    [SerializeField] private Color defaultColor = Color.white;

    [SerializeField] private int focusCounter;

    [SerializeField] private GameObject placeArea;
    [SerializeField] private IngredientBehaviour storedIngredient;

    [SerializeField] private float cookingTimer;
    [SerializeField] private float cookingSpeed = 1;
    [SerializeField] private bool isCooking;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        focusCounter = 0;
    }

    private void Start()
    {
        progressBarCanvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        if (isCooking)
        {
            float fillAmount = cookingTimer / storedIngredient.GetCookingTime();
            progressBarSlider.fillAmount = fillAmount;
            cookingTimer += cookingSpeed * Time.deltaTime;
        }
    }

    public void Interact()
    {
        if (storedIngredient != null && !storedIngredient.IsBurnt() && !isCooking)
            StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        progressBarCanvas.gameObject.SetActive(true);

        meshRenderer.material.color = Color.red;

        audioSource.PlayOneShot(oven);

        isCooking = true;
        animator.SetBool("isCooking", isCooking);

        cookingTimer = 0;

        yield return new WaitUntil(() => cookingTimer >= storedIngredient.GetCookingTime());

        audioSource.Stop();
        audioSource.PlayOneShot(ding);

        isCooking = false;
        animator.SetBool("isCooking", isCooking);

        storedIngredient.Cook();

        if (focusCounter < 1) meshRenderer.material.color = defaultColor;
        else meshRenderer.material.color = focusColor;

        progressBarCanvas.gameObject.SetActive(false);
    }

    public IngredientBehaviour GiveIngredient()
    {
        IngredientBehaviour pickedIngredient = storedIngredient;
        storedIngredient = null;
        return pickedIngredient;
    }

    public void StoreIngredient(IngredientBehaviour newIngredient)
    {
        storedIngredient = newIngredient;
        storedIngredient.gameObject.transform.SetParent(placeArea.transform);

        Vector3 placePosition = placeArea.transform.position;
        placePosition.y += newIngredient.gameObject.transform.localScale.y / 4;
        storedIngredient.transform.position = placePosition;
    }

    public bool IsCooking()
    {
        return isCooking;
    }

    public bool CanStore()
    {
        return storedIngredient == null;
    }

    public void Focus()
    {
        ++focusCounter;
        meshRenderer.material.color = focusColor;
    }

    public void Unfocus()
    {
        --focusCounter;
        if (focusCounter < 1) meshRenderer.material.color = defaultColor;
    }
}
