using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public InventoryObject inventoryObject;

    private InventoryObject.Dir dir = InventoryObject.Dir.Horizontal;

    private GridUI gridUI;
    public GameObject panel;
    public GameObject cellPrefab;
    private List<GameObject> inventorySlots = new List<GameObject>(); // for storing UI elements 

    public int gridWidth;
    public int gridHeight;
    public float cellSize; 

    private GameObject objectToMove;

    void Awake()
    {
        objectToMove = null;

        GenerateInventory();
    }

    void GenerateInventory()
    {
        // Initialize GridUI with panel, prefab, and grid size
        gridUI = new GridUI(panel, cellPrefab, gridWidth, gridHeight, new Vector2(100,100));

        // Store references to the generated grid cells
        foreach (Transform child in panel.transform)
        {
            GameObject slot = child.gameObject;
            inventorySlots.Add(slot);
            // Add an onClick listener to each slot
            slot.GetComponent<Button>().onClick.AddListener(() => OnInventorySlotClick(slot));
        }
    }

    void OnInventorySlotClick(GameObject slot)
    {
        // Debug.Log(slot.name + " clicked");

        Cell cellComp = slot.GetComponent<Cell>();
        Transform slotTransform = slot.transform;
        
        if(!cellComp.GetAvailable() && objectToMove == null) {
            if(slotTransform.childCount > 0) {
                objectToMove = slotTransform.GetChild(0).gameObject;
                objectToMove.transform.SetParent(null);
                // Debug.Log("moving " + objectToMove.name);
                
                // TODO: don't let objectToMove disappear when moving it 
                // (could try setting its parent to the mouse or an object that follows the mouse)

                PlacedObject placedObject = objectToMove.GetComponent<PlacedObject>();
                dir = placedObject.GetDir();
                inventoryObject = placedObject.GetInventoryObject();
                
                gridUI.RemoveItem(inventoryObject, dir, slot);

                return;
            }
            else {
                // TODO: figure out how to select the object anyway??
                Debug.Log("item is child of different slot");
                return;
            }
        }

        if(objectToMove != null) {
            if(gridUI.CanPlaceItem(inventoryObject, dir, slot)) {
                objectToMove.transform.SetParent(slotTransform);
                objectToMove.transform.localPosition = Vector3.zero;

                float rotation = inventoryObject.GetRotationAngle(dir);
                objectToMove.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
                
                gridUI.PlaceItem(inventoryObject, dir, slot);

                PlacedObject.SetUpObject(objectToMove, inventoryObject, dir);

                objectToMove = null; // reset reference
            }
        }

        // if(cellComp.GetAvailable()) {
        //     // if true then slot is empty
        //     Debug.Log("empty slot");
        // }
    }


    // change all logic past here
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            dir = InventoryObject.GetNextDir(dir);
            Debug.Log("current dir: " + dir);
        }

        if(Input.GetKeyDown(KeyCode.Q)) {
            AddObjectToInventory(inventoryObject);
        }
    }

    public void AddObjectToInventory(InventoryObject inventoryObject) 
    {
        foreach(GameObject slot in inventorySlots) {
            if(gridUI.CanPlaceItem(inventoryObject, dir, slot)) {
                // Debug.Log(slot + " empty");

                GameObject newItem = Instantiate(inventoryObject.prefab.gameObject, slot.transform);
                newItem.transform.localPosition = Vector3.zero;

                float rotation = inventoryObject.GetRotationAngle(dir);
                newItem.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

                // save the object when it's created
                PlacedObject.SetUpObject(newItem, inventoryObject, dir);

                gridUI.PlaceItem(inventoryObject, dir, slot);
                // Debug.Log("setting slot unavailable: " + slot.name);

                return;
            }
            else {
                Debug.Log(slot + " taken");
            }
        }
    }

    // for placing objects at specific inventory positions instead of next available
    // public void AddObjectToSpecific(InventoryObject inventoryObject, int x, int y, InventoryObject.Dir dir) 
    // {
    //     List<Vector2Int> gridPosList = inventoryObject.GetGridPositionList(new Vector2Int(x, y), dir);
    //     bool canPlace = true;
    //     foreach(Vector2Int gridPos in gridPosList) {
    //         if(gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= gridWidth || gridPos.y >= gridHeight) {
    //             canPlace = false; // out of bounds check
    //             break;
    //         }
    //         if(!grid.GetGridObject(gridPos.x, gridPos.y).CanPlace()) {
    //             canPlace = false; // overlap check
    //             break;
    //         }
    //     }

    //     if(canPlace) {
    //         Vector2Int rotationOffset = inventoryObject.GetRotationOffset(dir);
    //         Vector3 placedPos = new Vector3(x*10, y*10, 0) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * cellSize;

    //         PlacedObject placedObject = PlacedObject.Create(placedPos, new Vector2Int(x, y), dir, inventoryObject);
            
    //         foreach(Vector2Int gridPos in gridPosList) {
    //             grid.GetGridObject(gridPos.x, gridPos.y).SetPlacedObject(placedObject);
    //         }

    //         return;
    //     }
    // }

    public float GetCellSize()
    {
        return cellSize;
    }
}


// code for destroying an object (saving in case it's needed later)

// GridObject gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
// PlacedObject placedObject = gridObject.GetPlacedObject();
// if(placedObject != null) {
//     // can clear
//     placedObject.DestroySelf();

//     List<Vector2Int> gridPosList = placedObject.GetGridPositionList();
    
//     foreach(Vector2Int gridPos in gridPosList) {
//         grid.GetGridObject(gridPos.x, gridPos.y).ClearPlacedObject();
//     } 
// }