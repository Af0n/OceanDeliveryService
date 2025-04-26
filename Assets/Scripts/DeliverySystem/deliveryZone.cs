using UnityEngine;

public class deliveryZone : MonoBehaviour{
    public InventorySystem inventory;
    public Quest quest;
    [Tooltip("The reciepent name for the quest.")]
    public string recipientName;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            quest = QuestManager.Instance.GetQuestForRecipient(recipientName);
            Debug.Log($"Player entered delivery zone for quest: {quest.questName}");
            InventorySystem inventory = other.GetComponent<PlayerManager>().GetComponentInChildren<InventorySystem>();
            inventory.SetDeliveryZone(this);
            inventory.UpdateDeliveryZonePanel(quest.questName, recipientName);
        }
    }
    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            inventory.ClearDeliveryZone();
        }
    }
    public void CompleteDelivery(){
        quest = QuestManager.Instance.GetQuestForRecipient(recipientName);
        QuestManager.Instance.CompleteQuest(quest);
        Debug.Log($"Delivery completed for quest: {quest.questName}");
        gameObject.SetActive(false); 
    }
}
