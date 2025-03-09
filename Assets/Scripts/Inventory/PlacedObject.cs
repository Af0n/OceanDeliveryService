using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public static PlacedObject Create(Vector3 worldPos, Vector2Int origin, InventoryObject.Dir dir, InventoryObject inventoryObject)
    {
        InventorySystem inventorySystem = FindAnyObjectByType<InventorySystem>();

        Transform placedObjTransform = Instantiate(inventoryObject.prefab, worldPos, Quaternion.Euler(0, 0, inventoryObject.GetRotationAngle(dir)));
        // rescale prefab based on invenetory cell size
        placedObjTransform.localScale = new Vector3(inventoryObject.height * inventorySystem.GetCellSize(), inventoryObject.width * inventorySystem.GetCellSize(), 1);

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

    public InventoryObject GetInventoryObjectFromPlaced()
    {
        return inventoryObject;
    }
}
