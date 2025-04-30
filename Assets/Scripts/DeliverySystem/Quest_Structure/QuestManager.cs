using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance; // Singleton instance of the QuestManager

    void Awake()
    {
        instance = this;
    }

    public List<QuestData> activeQuests = new List<QuestData>(); // List of active quests
    public List<QuestData> completedQuests = new List<QuestData>(); // List of completed quests

    public List<(string questName, string description)> GetAllQuestsInfo()
    {
        List<(string questName, string description)> allQuestsInfo = new List<(string questName, string description)>();

        foreach (var quest in activeQuests)
        {
            allQuestsInfo.Add((quest.questName, quest.description));
        }

        foreach (var quest in completedQuests)
        {
            allQuestsInfo.Add((quest.questName, quest.description));
        }

        return allQuestsInfo;
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