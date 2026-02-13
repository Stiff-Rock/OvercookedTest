using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float orderingRate = 10.0f;
    [SerializeField] private float orderTimer;

    // Events
    public UnityEvent onCreateOrder;

    private void Start()
    {
        onCreateOrder.Invoke();
    }

    private void Update()
    {
        OrderTick();
    }

    private void OrderTick()
    {
        orderTimer += Time.deltaTime;

        if (orderTimer >= orderingRate)
        {
            orderTimer = 0;
            onCreateOrder.Invoke();
        }
    }
}