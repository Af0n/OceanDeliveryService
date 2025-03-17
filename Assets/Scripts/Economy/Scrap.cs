using UnityEngine;

public class Scrap : Interactable
{
    public int Value;
    public InventoryObject InventoryObject;

    public override void Interact()
    {
        Debug.Log("Here is where we would put the scrap into the inventory to then bring home\nFor now, auto processes");
        Process();
    }

    public void Process(){
        Economy.ChangeScrap(Value);
        Destroy(gameObject);
    }
}
