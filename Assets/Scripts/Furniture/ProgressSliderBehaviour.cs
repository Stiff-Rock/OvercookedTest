using UnityEngine;
using UnityEngine.UI;

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
