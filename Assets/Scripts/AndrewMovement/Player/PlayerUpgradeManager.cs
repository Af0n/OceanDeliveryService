using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Searcher;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("Water Resistance Upgrades")]
    public float waterResistanceUpgrade;
    public List<float> waterResistance = new List<float>();
    public int currentWaterIndex = 0;
    public int resistanceCost;
    
    [Header("Depth Resistance Upgrades")]
    public float depthUpgrade;
    public List<float> depthResistance = new List<float>();
    public int currentDepthIndex = 0;
    public int depthCost;

    [Header("Swim Ability Upgrades")]
    public int floatieCost;

    [Header("Inventory Capacity Upgrades")]
    public int inventoryCapacityUpgrade; // current capacity
    public List<int> inventoryCapacity = new List<int>(); // list of all sizes
    public int currentInventoryCapacityIndex = 0;
    public int inventoryCost;
    
    [Header("Swimming Speeds Upgrades")]
    public float swimSpeedUpgrade;
    public List<float> swimSpeed = new List<float>();
    public int currentSwimSpeedIndex = 0;
    public int speedCost;

    [Header("Goggles Upgrades")] 
    public bool hasGoggles;
    public int goggleCost;
    
    private AndrewMovement movement;
    private WaterDeath water;
    private InventorySystem inventory;

    void Start()
    {
        movement = GetComponent<AndrewMovement>();
        water = GetComponent<WaterDeath>();
        inventory = GetComponentInChildren<InventorySystem>();
    }

    public void GiveWaterResistanceUpgrade()
    {
        if (currentWaterIndex < waterResistance.Count)
        {
            waterResistanceUpgrade = waterResistance[currentWaterIndex];
            currentWaterIndex++;
            water.DrownTime = waterResistanceUpgrade;
        }
    }

    public void GiveDepthResistanceUpgrade()
    {
        if (currentDepthIndex < depthResistance.Count)
        {
            depthUpgrade = depthResistance[currentDepthIndex];
            currentDepthIndex++;   
        }
    }
    
    public void GiveSwimAbilityUpgrade()
    {
        movement.canSwim = true;
    }
    
    public void GiveInventoryUpgrade()
    {
        if (currentInventoryCapacityIndex < inventoryCapacity.Count)
        {
            inventoryCapacityUpgrade = inventoryCapacity[currentInventoryCapacityIndex];
            currentInventoryCapacityIndex++;

            inventory.UpgradeInventory(inventoryCapacityUpgrade);
        }
    }

    public void GiveSwimSpeedUpgrade()
    {
        if (currentSwimSpeedIndex < swimSpeed.Count)
        {
            swimSpeedUpgrade = swimSpeed[currentSwimSpeedIndex];
            currentSwimSpeedIndex++;
            movement.Speed = swimSpeedUpgrade;
        }
    }

    public void GiveGoggleUpgrade()
    {
        hasGoggles = true;
        
    }

    public int ListCosts(string name)
    {
        switch (name)
        {
            case "Water Resistance":
                return resistanceCost;

            case "Depth Resistance":
                return depthCost;

            case "Movement Speed":
                return speedCost;

            case "Floatie":
                return floatieCost;
            
            case "Goggles":
                return goggleCost;
            
            case "Inventory Size":
                return inventoryCost;

            default:
                return -1;
        }
    }

    public float ListCurrent(string name)
    {
        switch (name)
        {
            case "Water Resistance":
                return waterResistanceUpgrade;

            case "Depth Resistance":
                return depthUpgrade;

            case "Movement Speed":
                return swimSpeedUpgrade;

            case "Floatie":
                return -1;
            
            case "Goggles":
                return -1;
            
            case "Inventory Size":
                return inventoryCapacityUpgrade;

            default:
                return -1;
        }
    }
}
