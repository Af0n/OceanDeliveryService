using UnityEngine;

public class Package : Interactable
{
    public InventoryObject packageType;

    public override void Interact()
    {        
        InventorySystem inventory = FindAnyObjectByType<InventorySystem>();
        inventory.AddObjectToInventory(packageType);

        Destroy(gameObject); // might want to do something else based on what the game needs
    }
}
