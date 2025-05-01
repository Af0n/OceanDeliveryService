using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObjects/Quest", order = 1)]
public class QuestData : ScriptableObject
{
    public string questName;
    [TextArea] public string description; // Description of the quest
    public string recipientName; // Name of the recipient for the delivery
    public List<string> objectives; 
    public bool isCompleted;
}