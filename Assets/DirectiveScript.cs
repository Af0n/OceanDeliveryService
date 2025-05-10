using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;


public class DirectiveScript : MonoBehaviour
{
    public GameObject questEntryPrefab;
    //this variable is a pointer to content, which holds the questEntryPrefabs for our UI.
    public Transform contentParent;
    public bool database;

    void Start()
    {
        PopulateQuestList();
    }

    public void PopulateQuestList()
    {
        //clear out any questEntryPrefabs to prevent duplicates.
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
         
        //Adds our quests to a list to populate the UI later.
        List<QuestData> quests = new List<QuestData>();
        if (database)
        {
            quests.AddRange(QuestManager.instance.activeQuests);
        }
        else
        {
            quests.AddRange(QuestManager.instance.completedQuests);
        }

        foreach (var quest in quests)
        {
            //Makes a questEntryPrefab and calls function to populate text based on quest data.
            GameObject entryGO = Instantiate(questEntryPrefab, contentParent);
            QuestEntryUI entryUI = entryGO.GetComponent<QuestEntryUI>();
            entryUI.Initialize(quest);
        }
    }
}