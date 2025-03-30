using System;
using System.Collections.Generic;
using UnityEngine;

public class deliveryZone : MonoBehaviour
{
    [Header("Delivery Settings")]
    [Tooltip("The name of the quest attached to this zone.")]
    public QuestList QuestName;
    [Tooltip("The packages required to be delivered in this zone.")]
    public List<QuestRequirements> packages;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                // Notify the player about the required packages
                NotifyPlayer();
            }
        }
    }

    private void NotifyPlayer()
    {   
        Debug.Log("The quest attached to this Delivery Zone is: " + QuestName);
        Debug.Log("The quests attached to this Delivery Zone: " + string.Join(", ", packages));
        // You can add more code here to display the information in the UI
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