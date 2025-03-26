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
    public bool swimAbilityUpgrade;
    public int floatieCost;

    [Header("Inventory Capacity Upgrades")]
    public int inventoryCapacityUpgrade;
    public List<int> inventoryCapacity = new List<int>();
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

    public void GiveWaterResistanceUpgrade()
    {
        if (currentWaterIndex < waterResistance.Count)
        {
            waterResistanceUpgrade = waterResistance[currentWaterIndex];
            currentWaterIndex++;
            
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
        swimAbilityUpgrade = true;
    }
    
    public void GiveInventoryUpgrade()
    {
        if (currentInventoryCapacityIndex < inventoryCapacity.Count)
        {
            inventoryCapacityUpgrade = inventoryCapacity[currentInventoryCapacityIndex];
            currentInventoryCapacityIndex++;
        }
    }

    public void GiveSwimSpeedUpgrade()
    {
        if (currentSwimSpeedIndex < swimSpeed.Count)
        {
            swimSpeedUpgrade = swimSpeed[currentSwimSpeedIndex];
            currentSwimSpeedIndex++;
        }
    }

    public void GiveGoggleUpgrade()
    {
        hasGoggles = true;
    }
}
