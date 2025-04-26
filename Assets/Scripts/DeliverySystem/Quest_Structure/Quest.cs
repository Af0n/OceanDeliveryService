using System.Collections.Generic;
using UnityEngine;

public class Quest: MonoBehaviour{
    public string questName; 
    public string description;
    public string recipientName; 
    public bool isCompleted; 
    public List<string> objectives; 

    public Quest(string questName, string description, string recipientName, List<string> objectives)
    {
        this.questName = questName;
        this.description = description;
        this.recipientName = recipientName;
        this.objectives = objectives;
        this.isCompleted = false;
    }

    public void CompleteQuest()
    {
        isCompleted = true;
        Debug.Log($"Quest '{questName}' completed!");
    }
}