using System;
using System.Collections.Generic;
using UnityEngine;

public class deliveryZone : MonoBehaviour
{
    public InventorySystem inventory;
    [Tooltip("The reciepent name for the quest.")]
    public string recipientName;
    [Tooltip("The name of the quest attached to this zone.")]
    public QuestList QuestName;
    [Tooltip("The packages required to be delivered in this zone.")]
    public List<QuestRequirements> packages;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            inventory.UpdateDeliveryZonePanel(QuestName.ToString(), packages.ConvertAll(package => package.ToString()));
        }
    }

    private void NotifyPlayer()
    {   
        Debug.Log("The quest attached to this Delivery Zone is: " + QuestName);
        Debug.Log("The item requirements attached to this Delivery Zone: " + string.Join(", ", packages));
        // You can add more code here to display the information in the UI
    }
    public string GetQuestName(){
        return QuestName.ToString();
    }

    public string GetQuestRequirements(){
        return string.Join(", ", packages);
    }
    public enum QuestList{
        QuestA,
        QuestB,
        QuestC,
        //Just sample quests
    }

    public enum QuestRequirements
    {
        packageA,
        packageB,
        packageC,
        // Just sample packages, a quest struct should be made.
    }
}