using UnityEngine;

public class Order
{
    public float OrderTime { get; private set; }
    public Recipe Recipe { get; private set; }

    public int PointValue { get; private set; }

    public Order(Recipe recipe, int pointValue)
    {
        OrderTime = Time.time;
        Recipe = recipe;
        PointValue = pointValue;
    }
}
