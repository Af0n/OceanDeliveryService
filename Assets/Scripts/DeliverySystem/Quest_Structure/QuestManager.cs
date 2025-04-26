using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance; // Singleton instance

    public List<Quest> activeQuests = new List<Quest>(); 
    public List<Quest> completedQuests = new List<Quest>(); 

    private void Awake(){
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddQuest(Quest quest){
        activeQuests.Add(quest);
        Debug.Log($"Quest '{quest.questName}' added!");
    }

    // Mark a quest as completed
    public void CompleteQuest(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            quest.CompleteQuest();
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
        }
    }

    // Check if a delivery matches an active quest
    public Quest GetQuestForRecipient(string recipientName)
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.recipientName == recipientName && !quest.isCompleted)
            {
                return quest;
            }
        }
        return null;
    }
}