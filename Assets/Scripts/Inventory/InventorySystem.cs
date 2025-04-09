using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    private InventoryObject inventoryObject;

    private InventoryObject.Dir dir = InventoryObject.Dir.Horizontal;

    private GridUI gridUI;
    public GameObject panel;
    public GameObject DZPanel; 
    public GameObject cellPrefab;
    private List<GameObject> inventorySlots = new List<GameObject>(); // for storing UI elements 

    public int gridWidth;
    public int gridHeight;

    private GameObject objectToMove;
    public GameObject tempCell; // for holding objectToMove when applicable

    public Transform objDropPoint;

    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private TextMeshProUGUI title; // Declare title as a field
    private TextMeshProUGUI description; // Declare description as a field
    private TextMeshProUGUI questName; // Declare questName as a field
    private bool isDisplayed; // so inputs aren't called unless the inventory is shown (not that they do anything if it isn't)

    // input system stuff
    private InputSystem_Actions actions;
    private InputAction rotate, drop; 

    void Awake()
    {
        actions = new InputSystem_Actions();
        objectToMove = null;
        GenerateInventory();
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();

        // make sure inventory is created but uninteractable
        canvasGroup = GetComponent<CanvasGroup>();
        DisplayInventory(false);
    }

    public void DisplayInventory(bool toDisplay)
    {
        // here bc call in PauseUI runs before canvasGroup is assigned
        if(canvasGroup == null) {
            return;
        }

        if(toDisplay) {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            isDisplayed = true;
        }
        else {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            isDisplayed = false;
        }
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
    public void UpdateDZPanelState(bool isInDeliveryZone, bool isOnInventoryTab){
       DZPanel.SetActive(isInDeliveryZone && isOnInventoryTab);
    }

    void OnInventorySlotClick(GameObject slot)
    {
        // Debug.Log(slot.name + " clicked");

        Cell cellComp = slot.GetComponent<Cell>();
        
        // pick up an inventory object
        if(!cellComp.GetAvailable() && objectToMove == null) {
            Transform rootCell = cellComp.GetRoot();

            PlacedObject placedObject = rootCell.GetComponentInChildren<PlacedObject>();
            dir = placedObject.GetDir();
            inventoryObject = placedObject.GetInventoryObject();

            objectToMove = placedObject.gameObject;
            objectToMove.transform.SetParent(tempCell.transform);
            objectToMove.transform.localPosition = Vector3.zero;
            
            gridUI.RemoveItem(inventoryObject, dir, slot);

            return;
        }

        // place an inventory object
        if(objectToMove != null) {
            if(gridUI.CanPlaceItem(inventoryObject, dir, slot)) {
                objectToMove.transform.SetParent(slot.transform);
                objectToMove.transform.localPosition = Vector3.zero;
                
                gridUI.PlaceItem(inventoryObject, dir, slot);

                PlacedObject.SetUpObject(objectToMove, inventoryObject, dir);

                objectToMove = null; // reset reference
            }
        }
    }

    void Update()
    {
        // so objectToMove follows the mouse when selected
        if(objectToMove != null) {
            Vector2 pos;
            RectTransform inventoryPanelRect = panel.GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                Input.mousePosition,
                canvas.worldCamera,
                out pos
            );

            Vector2 minPosition = inventoryPanelRect.rect.min + (Vector2)objectToMove.transform.localScale * 0.5f;
            Vector2 maxPosition = inventoryPanelRect.rect.max - (Vector2)objectToMove.transform.localScale * 0.5f;

            // clamp position to inventory menu (makes it look nicer)
            pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);

            tempCell.transform.position = canvas.transform.TransformPoint(pos);
        }
    }

    public bool AddObjectToInventory(InventoryObject inventoryObject) 
    {
        foreach(GameObject slot in inventorySlots) {
            if(gridUI.CanPlaceItem(inventoryObject, dir, slot)) {
                // Debug.Log(slot + " empty");

                GameObject newItem = Instantiate(inventoryObject.uiPrefab, slot.transform);
                newItem.transform.localPosition = Vector3.zero;

                float rotation = inventoryObject.GetRotationAngle(dir);
                newItem.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

                // save the object when it's created
                PlacedObject.SetUpObject(newItem, inventoryObject, dir);

                gridUI.PlaceItem(inventoryObject, dir, slot);
                // Debug.Log("setting slot unavailable: " + slot.name);

                return true;
            }
            else {
                Debug.Log(slot + " taken");
            }
        }

        return false;
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        if(objectToMove != null && isDisplayed) {
            dir = InventoryObject.GetNextDir(dir);
            float rotation = inventoryObject.GetRotationAngle(dir);
            objectToMove.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }

    private void Drop(InputAction.CallbackContext context)
    {
        if(objectToMove != null && isDisplayed) {
            PlacedObject placedObject = objectToMove.GetComponent<PlacedObject>();
            InventoryObject newObject = placedObject.GetInventoryObject();
            Instantiate(newObject.worldPrefab, objDropPoint.position, objDropPoint.rotation);

            // reset inventory movement stuff
            ResetObjectToMove();
        }
    }

    // will place objectToMove back in first available pos in inventory (for pause menu logic)
    public void CheckIfMoving()
    {
        if(objectToMove != null) {
            AddObjectToInventory(inventoryObject);
            ResetObjectToMove();
        }
    }

    private void ResetObjectToMove()
    {
        Destroy(objectToMove);
        objectToMove = null; 
    }

    void OnEnable()
    {
        // input system boilerplate
        rotate = actions.UI.RotateInventory;
        rotate.Enable();
        rotate.performed += Rotate;

        drop = actions.UI.DropInventory;
        drop.Enable();
        drop.performed += Drop;
    }

    void OnDisable()
    {
        // input system boilerplate
        rotate.Disable(); // null reference here
        drop.Disable();
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