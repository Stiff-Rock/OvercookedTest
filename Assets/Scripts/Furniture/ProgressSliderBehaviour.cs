using UnityEngine;
using UnityEngine.UI;

// TODO: TODOS LOS CANVAS DEBERIAN ESTAR ORIENTADOS A LA CAMARA PRINCIPAL
public class ProgressSliderBehaviour : MonoBehaviour
{
    [Header("GUI")]
    [SerializeField] private Canvas progressBarCanvas;
    [SerializeField] private Image progressBarSlider;

    private void Start()
    {
        progressBarCanvas.worldCamera = Camera.main;
    }

    public void UpdateProgressBar(IngredientBehaviour placedIngredient)
    {
        progressBarSlider.fillAmount = placedIngredient.GetCookedTime() / placedIngredient.GetRemainingTime();
    }

    public void SetActive(bool active)
    {
        progressBarCanvas.gameObject.SetActive(active);
    }
}
