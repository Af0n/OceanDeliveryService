using UnityEngine;

public class deliveryZone : MonoBehaviour
{
    public InventorySystem inventory;
    [Tooltip("Assign the quest data for this delivery zone.")]
    public QuestData questData; // Reference to the ScriptableObject containing quest information
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            Debug.Log($"Player entered delivery zone for quest: {questData.questName}");
            InventorySystem inventory = other.GetComponent<PlayerManager>().GetComponentInChildren<InventorySystem>();
            inventory.SetDeliveryZone(this);
            inventory.UpdateDeliveryZonePanel(questData.questName, questData.objectives);
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            inventory.ClearDeliveryZone();
        }
    }

    public void CompleteDelivery(){
        questData.isCompleted = true;
        Debug.Log($"Delivery completed for quest: {questData.questName}");
        gameObject.SetActive(false); // Disable the delivery zone
    }
}