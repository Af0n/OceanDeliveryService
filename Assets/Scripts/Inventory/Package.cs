using UnityEngine;

public class Package : Interactable
{
    public InventoryObject packageType;
    public string reciepent;
    public QuestData questData;

    void Start()
    {
        if(reciepent == null) {
            Debug.LogError("no package recipient for " + gameObject.name);
        }
    }

    public override void Interact()
    {        
        InventorySystem inventory = FindAnyObjectByType<InventorySystem>();
        bool successful = inventory.AddObjectToInventory(packageType, reciepent);
        
        // cancel interaction if there's no inventory space
        if(!successful) {
            Debug.Log("inventory full!"); 
            return;
        }
        
        QuestManager.instance.AddQuest(questData);

        Destroy(gameObject);
    }

    public void SetRecipient(string name)
    {
        reciepent = name;
    }
}
