using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public static PlacedObject Create(Vector3 worldPos, Vector2Int origin, InventoryObject.Dir dir, InventoryObject inventoryObject)
    {
        Transform placedObjTransform = Instantiate(inventoryObject.prefab, worldPos, Quaternion.Euler(0, 0, inventoryObject.GetRotationAngle(dir)));
        
        PlacedObject placedObject = placedObjTransform.GetComponent<PlacedObject>();

        placedObject.inventoryObject = inventoryObject;
        placedObject.origin = origin;
        placedObject.dir = dir;

        return placedObject;
    }

    private InventoryObject inventoryObject;
    private Vector2Int origin;
    private InventoryObject.Dir dir;

    public List<Vector2Int> GetGridPositionList()
    {
        return inventoryObject.GetGridPositionList(origin, dir);
    }
    
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
