using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public static void SetUpObject(GameObject itemSource, InventoryObject inventoryObject, InventoryObject.Dir dir, string reciepent) 
    {
        PlacedObject placedObject = itemSource.GetComponent<PlacedObject>();
        placedObject.inventoryObject = inventoryObject;
        placedObject.dir = dir;
        placedObject.reciepent = reciepent;
    }

    private InventoryObject inventoryObject;
    private InventoryObject.Dir dir;
    [SerializeField]
    private string reciepent; // set as serialize field so viewable from inspector (for testing)

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public InventoryObject GetInventoryObject()
    {
        return inventoryObject;
    }

    public InventoryObject.Dir GetDir()
    {
        return dir;
    }

    public string GetRecipient()
    {
        return reciepent;
    }
}
