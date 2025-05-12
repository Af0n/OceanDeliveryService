using UnityEngine;

public class SmeltStation : Interactable
{
    public InventorySystem scrap;
    public override void Interact()
    {
        scrap.SellScrap();
    }
}
