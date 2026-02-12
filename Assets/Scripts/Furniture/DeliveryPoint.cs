using UnityEngine;

public class DeliveryPoint : InteractiveAppliance
{
    private KitchenOrdersManager ordersManager;

    protected override void Start()
    {
        base.Start();

        ordersManager = GameObject
            .FindWithTag("KitchenOrdersManager")
            .GetComponent<KitchenOrdersManager>();
    }

    public void ServeOrder(Recipe recipe)
    {
        ordersManager.ServeDish(recipe);
    }

    public override bool HasItem()
    {
        return true;
    }
}
