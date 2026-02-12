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

    public void UpdateProgressBar(float progress)
    {
        progressBarSlider.fillAmount = progress;
    }

    public void SetActive(bool active)
    {
        progressBarCanvas.gameObject.SetActive(active);
    }
}
