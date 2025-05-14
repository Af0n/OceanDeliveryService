using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public event System.Action OnPackageDelivered;
    private InventoryObject inventoryObject;
    private deliveryZone deliveryZone;
    private InventoryObject.Dir dir = InventoryObject.Dir.Horizontal;

    private GridUI gridUI;
    public GameObject panel;
    public GameObject DZPanel;
    public GameObject cellPrefab;
    private List<GameObject> inventorySlots = new List<GameObject>(); // for storing UI elements 

    public int gridHeight;
    public int gridWidth;

    private GameObject objectToMove;
    public GameObject tempCell; // for holding objectToMove when applicable

    public Transform objDropPoint;

    private CanvasGroup canvasGroup;
    private Canvas canvas;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questRequirementsText;
    public TextMeshProUGUI errorMessageText;
    public Button deliverButton;
    public Creator questFlag;

    private bool
        isDisplayed; // so inputs aren't called unless the inventory is shown (not that they do anything if it isn't)

    // input system stuff
    private InputSystem_Actions actions;
    private InputAction rotate, drop;

    void Awake()
    {
        actions = new InputSystem_Actions();
        objectToMove = null;

        // get correct size based on upgrades 
        gridWidth = GetComponentInParent<PlayerUpgradeManager>().inventoryCapacityUpgrade;

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
        if (canvasGroup == null)
        {
            return;
        }

        if (toDisplay)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isDisplayed = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            isDisplayed = false;
        }
    }

    void GenerateInventory()
    {
        // clear old slots if they exist (when upgrading)
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot);
        }

        inventorySlots.Clear();

        // Initialize GridUI with panel, prefab, and grid size
        gridUI = new GridUI(panel, cellPrefab, gridHeight, gridWidth, new Vector2(100, 100));

        // Store references to the generated grid cells
        inventorySlots = gridUI.GetAllCells();
        foreach (GameObject slot in inventorySlots)
        {
            Button button = slot.GetComponent<Button>();
            button.onClick.RemoveAllListeners(); // Prevent duplicate handlers
            button.onClick.AddListener(() => OnInventorySlotClick(slot));
        }
    }

    public void SetDeliveryZone(deliveryZone deliveryZone)
    {
        this.deliveryZone = deliveryZone;
    }

    public void ClearDeliveryZone()
    {
        deliveryZone = null;
    }
    public void UpdateDeliveryZonePanel(string questName, List<string> questObjectives){
        questRequirementsText.text = string.Empty;
        questNameText.text = questName;
        questRequirementsText.text = string.Empty;
        foreach (string objective in questObjectives){
            questRequirementsText.text += "· " + objective + "\n";
        }
    }

    void OnInventorySlotClick(GameObject slot)
    {
        Debug.Log(slot.name + " clicked");

        Cell cellComp = slot.GetComponent<Cell>();

        // pick up an inventory object
        if (!cellComp.GetAvailable() && objectToMove == null)
        {
            Transform rootCell = cellComp.GetRoot();
            // Debug.Log(rootCell);

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
        if (objectToMove != null)
        {
            if (gridUI.CanPlaceItem(inventoryObject, dir, slot))
            {
                objectToMove.transform.SetParent(slot.transform);
                objectToMove.transform.localPosition = Vector3.zero;

                string name = objectToMove.GetComponent<PlacedObject>().GetRecipient();
                Debug.Log("recipient is " + name);

                gridUI.PlaceItem(inventoryObject, dir, slot);

                PlacedObject.SetUpObject(objectToMove, inventoryObject, dir, name);

                objectToMove = null; // reset reference
            }
        }
    }

    void Update()
    {
        // so objectToMove follows the mouse when selected
        if (objectToMove != null)
        {
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

    public void UpgradeInventory(int newSize)
    {
        // store any objects in tempCell before changing inventory size
        foreach (GameObject slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                GameObject childObj = slot.transform.GetChild(0).gameObject;
                childObj.transform.SetParent(tempCell.transform);
                childObj.transform.localPosition = Vector3.zero;

                PlacedObject placedObject = childObj.GetComponent<PlacedObject>();
                dir = placedObject.GetDir();
                inventoryObject = placedObject.GetInventoryObject();

                gridUI.RemoveItem(inventoryObject, dir, slot);
            }
        }

        // recreate inventory w/ new size
        gridUI.ClearGrid();
        gridWidth = newSize;
        GenerateInventory();

        // put all stored objects back in inventory
        foreach (Transform child in tempCell.transform)
        {
            GameObject childObj = child.gameObject;
            PlacedObject placedObject = childObj.GetComponent<PlacedObject>();
            InventoryObject inventoryObject = placedObject.GetInventoryObject();
            string reciepent = placedObject.GetRecipient();
            dir = placedObject.GetDir(); // to make sure dir is correct 

            AddObjectToInventory(inventoryObject, reciepent);

            Destroy(childObj); // bc AddObjectToInventory creates a duplicate so the old one needs to go
        }
    }

    public bool AddObjectToInventory(InventoryObject inventoryObject, string reciepentName)
    {
        foreach (GameObject slot in inventorySlots)
        {
            if (gridUI.CanPlaceItem(inventoryObject, dir, slot))
            {
                GameObject newItem = Instantiate(inventoryObject.uiPrefab, slot.transform);
                newItem.transform.localPosition = Vector3.zero;

                float rotation = inventoryObject.GetRotationAngle(dir);
                newItem.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

                // save the object when it's created
                PlacedObject.SetUpObject(newItem, inventoryObject, dir, reciepentName);

                gridUI.PlaceItem(inventoryObject, dir, slot);
                // Debug.Log("setting slot unavailable: " + slot.name);

                return true;
            }
            else
            {
                Debug.Log("could not find slot -- see GridUI.CanPlaceItem()");
            }
        }

        return false;
    }
    
    public bool AddScrapToInventory(InventoryObject inventoryObject, int value)
    {
        foreach (GameObject slot in inventorySlots)
        {
            if (gridUI.CanPlaceItem(inventoryObject, dir, slot))
            {
                GameObject newItem = Instantiate(inventoryObject.uiPrefab, slot.transform);
                newItem.transform.localPosition = Vector3.zero;

                float rotation = inventoryObject.GetRotationAngle(dir);
                newItem.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

                // save the object when it's created
                PlacedObject.SetUpScrap(newItem, inventoryObject, dir, value);

                gridUI.PlaceItem(inventoryObject, dir, slot);
                // Debug.Log("setting slot unavailable: " + slot.name);

                return true;
            }
            else
            {
                Debug.Log("could not find slot -- see GridUI.CanPlaceItem()");
            }
        }

        return false;
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        if (objectToMove != null && isDisplayed)
        {
            dir = InventoryObject.GetNextDir(dir);
            float rotation = inventoryObject.GetRotationAngle(dir);
            objectToMove.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }

    private void Drop(InputAction.CallbackContext context)
    {
        if (objectToMove != null && isDisplayed)
        {
            PlacedObject placedObject = objectToMove.GetComponent<PlacedObject>();
            InventoryObject newObject = placedObject.GetInventoryObject();
            GameObject droppedObj = Instantiate(newObject.worldPrefab, objDropPoint.position, objDropPoint.rotation);

            Package package = droppedObj.GetComponent<Package>();
            if (package != null)
            {
                package.SetRecipient(placedObject.GetRecipient());
            }

            // reset inventory movement stuff
            ResetObjectToMove();
        }
    }

    // will place objectToMove back in first available pos in inventory (for pause menu logic)
    public void CheckIfMoving()
    {
        if (objectToMove != null)
        {
            AddObjectToInventory(inventoryObject, objectToMove.GetComponent<PlacedObject>().GetRecipient());
            ResetObjectToMove();
        }
    }

    private void ResetObjectToMove()
    {
        Destroy(objectToMove);
        objectToMove = null;
    }

    // for package delivery
    public void DeliverPackage(){
        if (deliveryZone == null || deliveryZone.questData == null){
            Debug.LogError("Delivery zone or quest data is null!");
            return;
        }
        string questRecipientName = deliveryZone.questData.recipientName;
        Debug.Log("questRecipientName: " + questRecipientName);
        foreach (GameObject slot in inventorySlots){

             // Check if the slot has a child and the child is tagged as "Package"
            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).CompareTag("Package")){
                string packageRecipient = slot.transform.GetChild(0).GetComponent<PlacedObject>().GetRecipient();
                Debug.Log("packageRecipient: " + packageRecipient + ", questRecipientName: " + questRecipientName);

                // Check if the recipient name matches the quest name
                if (packageRecipient == questRecipientName){
                    Cell cellComp = slot.GetComponent<Cell>();
                    Transform rootCell = cellComp.GetRoot();
                    PlacedObject tempPlacedObject = rootCell.GetComponentInChildren<PlacedObject>();
                    dir = tempPlacedObject.GetDir();
                    inventoryObject = tempPlacedObject.GetInventoryObject();
                    objectToMove = tempPlacedObject.gameObject;

                    // Instantiate the dropped object in the world
                    GameObject droppedObj = Instantiate(inventoryObject.worldPrefab, objDropPoint.position, objDropPoint.rotation);
                    droppedObj.GetComponent<Package>().SetRecipient(packageRecipient);

                    // Remove the item from the grid and reset
                    gridUI.RemoveItem(inventoryObject, dir, slot);
                    ResetObjectToMove();
                    // Trigger the delivery event
                    OnPackageDelivered?.Invoke();
                    // Mark the delivery as complete
                    deliveryZone.CompleteDelivery();
                    ClearDeliveryZone();
                    questFlag.CompleteQuest();
                    Economy.Scrap += 30;
                    return;
                }
            }
        }
        // Show an error message if no matching package was found
        errorMessageText.gameObject.SetActive(true);
        Invoke(nameof(HideErrorMsg), 1f);
    }

    private void HideErrorMsg()
    {
        errorMessageText.gameObject.SetActive(false);
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

    public void SellScrap()
    {
        foreach (GameObject slot in inventorySlots)
        {
            // Check if the slot has a child and that child has a Scrap component
            if (slot.transform.childCount > 0)
            {
                if(slot.transform.GetChild(0).GetComponent<PlacedObject>().GetValue() != 0)
                {
                    Cell cellComp = slot.GetComponent<Cell>();
                    Transform rootCell = cellComp.GetRoot();
                    PlacedObject tempPlacedObject = rootCell.GetComponentInChildren<PlacedObject>();
                    dir = tempPlacedObject.GetDir();
                    inventoryObject = tempPlacedObject.GetInventoryObject();
                    objectToMove = tempPlacedObject.gameObject;

                    Economy.ChangeScrap(slot.transform.GetChild(0).GetComponent<PlacedObject>().GetValue());

                    // Remove the item from the grid and reset
                    gridUI.RemoveItem(inventoryObject, dir, slot);
                    ResetObjectToMove();
                }
            }
        }
    }
}
    