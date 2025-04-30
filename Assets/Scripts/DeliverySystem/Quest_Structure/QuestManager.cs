using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance; // Singleton instance

    public List<QuestData> activeQuests = new List<QuestData>(); // List of active quests
    public List<QuestData> completedQuests = new List<QuestData>(); // List of completed quests

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add a new quest to the active quests list
    public void AddQuest(QuestData questData)
    {
        if (!activeQuests.Contains(questData) && !questData.isCompleted)
        {
            activeQuests.Add(questData);
            Debug.Log($"Quest '{questData.questName}' added to active quests!");
        }
    }

    // Mark a quest as completed
    public void CompleteQuest(QuestData questData)
    {
        if (activeQuests.Contains(questData))
        {
            questData.isCompleted = true;
            activeQuests.Remove(questData);
            completedQuests.Add(questData);
            Debug.Log($"Quest '{questData.questName}' marked as completed!");
        }
    }

    // Check if a delivery matches an active quest
    public QuestData GetQuestForRecipient(string recipientName)
    {
        foreach (QuestData questData in activeQuests)
        {
            if (questData.recipientName == recipientName && !questData.isCompleted)
            {
                return questData;
            }
        }
        return null;
    }
}