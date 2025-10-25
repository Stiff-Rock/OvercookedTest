using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class InteractObject : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip oven;
    [SerializeField] private AudioClip ding;

    [SerializeField] private Color focusColor = Color.yellow;
    [SerializeField] private Color defaultColor = Color.white;

    [SerializeField] private int focusCounter;

    [SerializeField] private GameObject placeArea;
    [SerializeField] private IngredientBehaviour storedIngredient;


    [SerializeField] private float cookingTime;
    [SerializeField] private bool isCooking;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        focusCounter = 0;
    }

    public void Interact()
    {
        if (storedIngredient != null && !storedIngredient.IsBurnt() && !isCooking)
            StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        meshRenderer.material.color = Color.red;

        audioSource.PlayOneShot(oven);

        isCooking = true;
        animator.SetBool("isCooking", isCooking);

        yield return new WaitForSeconds(cookingTime);

        audioSource.Stop();
        audioSource.PlayOneShot(ding);

        isCooking = false;
        animator.SetBool("isCooking", isCooking);

        storedIngredient.Cook();

        if (focusCounter < 1) meshRenderer.material.color = defaultColor;
        else meshRenderer.material.color = focusColor;
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
