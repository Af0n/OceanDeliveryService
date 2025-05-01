using UnityEngine;

public class QuestStartZone : MonoBehaviour
{
    [Tooltip("Assign the quest data for this quest start zone.")]
    public QuestData questData; // Reference to the ScriptableObject containing quest information
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            QuestManager.instance.AddQuest(questData); // Add the quest to the active quests when the player enters the zone(will be changed to add when the player accepts the quest)
            Debug.Log($"Player entered quest start zone for quest: {questData.questName}");
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            this.gameObject.SetActive(false); // Disable the quest start zone(will be changed to disable when the player accepts the quest)
        }
    }

}