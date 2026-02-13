using UnityEngine;
using UnityEngine.UI;

public class ProgressSliderBehaviour : MonoBehaviour
{
    [Header("GUI")]
    [SerializeField] private Canvas progressBarCanvas;
    [SerializeField] private Image progressBarSlider;

    private void Awake()
    {
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.y = Mathf.RoundToInt(currentEuler.y);

        if (Mathf.Approximately(currentEuler.y, 90f) || Mathf.Approximately(currentEuler.y, 270f))
        {
            Vector3 parentEuler = progressBarCanvas.transform.localEulerAngles;
            parentEuler.y = -currentEuler.y;
            progressBarCanvas.transform.localEulerAngles = parentEuler;
        }
    }

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
