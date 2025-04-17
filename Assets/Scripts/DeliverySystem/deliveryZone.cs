using System;
using System.Collections.Generic;
using UnityEngine;

public class deliveryZone : MonoBehaviour{
    public InventorySystem inventory;
    [Tooltip("The reciepent name for the quest.")]
    public string recipientName;
    [Tooltip("The name of the quest attached to this zone.")]
    public QuestList QuestName;
    [Tooltip("The packages required to be delivered in this zone.(features implementation is TBD)")]
    // public List<QuestRequirements> packages;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            inventory.SetDeliveryZone(this);
            inventory.UpdateDeliveryZonePanel(QuestName.ToString(), recipientName);
            // inventory.UpdateDeliveryZonePanel(QuestName.ToString(), packages.ConvertAll(package => package.ToString()));
            
        }
    }
    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            inventory.ClearDeliveryZone();
        }
    }
    public void DisableDeliveryZone(){
        gameObject.SetActive(false);
    }
    // public string GetQuestRequirements(){
    //     return string.Join(", ", packages);
    // }
    public enum QuestList{
        QuestA,
        QuestB,
        QuestC,
        //Just sample quests
    }
    

    // public enum QuestRequirements
    // {
    //     packageA,
    //     packageB,
    //     packageC,
    //     // Just sample packages, a quest struct should be made.
    // }
}