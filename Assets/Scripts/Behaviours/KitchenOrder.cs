using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KitchenOrder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image recipeImage;
    [SerializeField] private TextMeshProUGUI dishNameText;
    [SerializeField] private Image lifetimeProgressBar;
    private Color initialBarColor;

    [Header("Properties")]
    [SerializeField] private float maxLifespan;
    [field: SerializeField] public float Lifespan { get; private set; }
    [field: SerializeField] public Recipe Recipe { get; private set; }
    [SerializeField] private bool isCompleted;

    [Header("Events")]
    public UnityEvent OnExpire;

    private void Awake()
    {
        initialBarColor = lifetimeProgressBar.color;
        enabled = false;
    }

    public void Initialize(Recipe recipe, float lifespan)
    {
        Recipe = recipe;
        maxLifespan = lifespan;
        Lifespan = lifespan;

        // Assing image and name
        // recipeImage
        dishNameText.SetText(recipe.DishType.ToString());

        enabled = true;
    }

    private void Update()
    {
        Lifespan -= Time.deltaTime;

        float progress = Mathf.Clamp01(Lifespan / maxLifespan);
        lifetimeProgressBar.fillAmount = progress;
        lifetimeProgressBar.color = Color.Lerp(Color.red, initialBarColor, progress);

        if (Lifespan <= 0)
        {
            Lifespan = 0;

            isCompleted = false;
            OnExpire.Invoke();
            enabled = false;
        }
    }

    #region Getters and Setters

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void SetIsCompleted(bool isCompleted)
    {
        enabled = !isCompleted;
        this.isCompleted = isCompleted;
    }

    #endregion
}