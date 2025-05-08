using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance; // Singleton instance of the QuestManager

    void Awake()
    {
        instance = this;
    }

    public List<QuestData> activeQuests = new List<QuestData>(); // List of active quests
    public List<QuestData> completedQuests = new List<QuestData>(); // List of completed quests
    // Events to notify when quests are added or completed
    public event Action OnQuestAdded;
    public event Action OnQuestCompleted;
    public List<(string questName, List<string> objectives)> GetAllQuestsInfo()
    {
        List<(string questName, List<string> objectives)> allQuestsInfo = new List<(string questName, List<string> objectives)>();

        foreach (var quest in activeQuests)
        {
            allQuestsInfo.Add((quest.questName, quest.objectives));
        }

        foreach (var quest in completedQuests)
        {
            allQuestsInfo.Add((quest.questName, quest.objectives));
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
            // Trigger the OnQuestAdded event
            OnQuestAdded?.Invoke();
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
            // Trigger the OnQuestCompleted event
            OnQuestCompleted?.Invoke();
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