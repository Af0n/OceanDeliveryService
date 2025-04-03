using UnityEngine;

public class Package : Interactable
{
    public InventoryObject packageType;

    public override void Interact()
    {        
        InventorySystem inventory = FindAnyObjectByType<InventorySystem>();
        bool successful = inventory.AddObjectToInventory(packageType);
        
        // cancel interaction if there's no inventory space
        if(!successful) {
            Debug.Log("inventory full!"); 
            return;
        }

        Destroy(gameObject);
    }
}
