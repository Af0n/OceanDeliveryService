using System.Collections.Generic;
using UnityEngine;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent; // Parent object for the quest buttons

    [Header("Rect Transforms")]
    [SerializeField] private RectTransform scrollRectTransform;
    [SerializeField] private RectTransform contentRectTransform;

    [Header("Quest Log Button")]
    [SerializeField] private GameObject questLogButtonPrefab; // Prefab for the quest log button

    private Dictionary<string, QuestLogButton> idToButtonMap = new Dictionary<string, QuestLogButton>();

    private void Start()
    {
        // Subscribe to quest events
        QuestManager.instance.OnQuestAdded += PopulateQuestLog;
        QuestManager.instance.OnQuestCompleted += PopulateQuestLog;

        // Populate the quest log initially
        PopulateQuestLog();
    }

    private void OnDestroy()
    {
        // Unsubscribe from quest events to avoid memory leaks
        if (QuestManager.instance != null)
        {
            QuestManager.instance.OnQuestAdded -= PopulateQuestLog;
            QuestManager.instance.OnQuestCompleted -= PopulateQuestLog;
        }
    }

    // Populate the quest log with active and completed quests
    private void PopulateQuestLog()
    {
        // Clear existing buttons
        foreach (Transform child in contentParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Add active quests
        foreach (QuestData quest in QuestManager.instance.activeQuests)
        {
            CreateButtonIfNotExists(quest);
        }

        // Add completed quests
        foreach (QuestData quest in QuestManager.instance.completedQuests)
        {
            CreateButtonIfNotExists(quest);
        }
    }

    // Create a button for a quest if it doesn't already exist
    public QuestLogButton CreateButtonIfNotExists(QuestData quest)
    {
        QuestLogButton questLogButton = null;

        // Only create the button if it hasn't been created yet
        if (!idToButtonMap.ContainsKey(quest.questName))
        {
            questLogButton = InstantiateQuestLogButton(quest);
        }
        else
        {
            questLogButton = idToButtonMap[quest.questName];
        }

        return questLogButton;
    }

    // Instantiate a quest log button
    private QuestLogButton InstantiateQuestLogButton(QuestData quest)
    {
        // Create the button
        QuestLogButton questLogButton = Instantiate(
            questLogButtonPrefab,
            contentParent.transform).GetComponent<QuestLogButton>();

        // Set the button's name and initialize it
        questLogButton.gameObject.name = quest.questName + "_button";
        questLogButton.Initialize(quest.questName, null); // Only display the quest name, no action needed

        // Add to the map to keep track of the new button
        idToButtonMap[quest.questName] = questLogButton;
        return questLogButton;
    }
}