using UnityEngine;

public class Scrap : Interactable
{
    public int Value;
    public InventoryObject InventoryObject;

    public override void Interact()
    {
        InventorySystem inventory = FindAnyObjectByType<InventorySystem>();
        bool successful = inventory.AddScrapToInventory(InventoryObject, Value); // give it an emtpy string so I can reuse the same function (it won't matter)
        
        // cancel interaction if there's no inventory space
        if(!successful) {
            Debug.Log("inventory full!"); 
            return;
        }

        Process();
    }

    public void Process(){
        // Economy.ChangeScrap(Value);
        Destroy(gameObject);
    }
}
