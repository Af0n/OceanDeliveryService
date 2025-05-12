using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FusionForge : MonoBehaviour
{
    public List<GameObject> buttons = new List<GameObject>();
    
    public BoatUpgradeManager boatUpgradeManager;
    public PlayerUpgradeManager playerUpgradeManager;
    
    public GameObject playerTab;
    public GameObject vehicleTab;

    public void Awake()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            TextMeshProUGUI tmp = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (i <= 5)
            {
                string value;
                if (playerUpgradeManager.ListCurrent(buttons[i].name) == -1)
                {
                    value = "";
                }
                else
                {
                    value = playerUpgradeManager.ListCurrent(buttons[i].name).ToString();
                }
                tmp.text = value + " " + buttons[i].name + ": " + playerUpgradeManager.ListCosts(buttons[i].name) + " Scrap Currency";
            }
            else
            {
                string value;
                if (boatUpgradeManager.ListCurrent(buttons[i].name) == -1)
                {
                    value = "";
                }
                else
                {
                    value = boatUpgradeManager.ListCurrent(buttons[i].name).ToString();
                }
                tmp.text = value + " " + buttons[i].name + ": " + boatUpgradeManager.ListCosts(buttons[i].name) + " Scrap Currency";
            }
            
        }
    }
    
    public void PlayerTab()
    {
        vehicleTab.SetActive(false);
        playerTab.SetActive(true);
    }

    public void VehicleTab()
    {
        playerTab.SetActive(false);
        vehicleTab.SetActive(true);
    }
    
    public void PlayerUpgradeResistance()
    {
        if (Economy.Scrap >= playerUpgradeManager.resistanceCost && playerUpgradeManager.currentWaterIndex < playerUpgradeManager.waterResistance.Count)
        {
            playerUpgradeManager.GiveWaterResistanceUpgrade();
            Economy.TakeScrap(playerUpgradeManager.resistanceCost);
            if (playerUpgradeManager.currentWaterIndex == playerUpgradeManager.waterResistance.Count)
            {
                buttons[0].SetActive(false);
                return;
            }
            TextMeshProUGUI tmp = buttons[0].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = playerUpgradeManager.waterResistance[playerUpgradeManager.currentWaterIndex - 1] + " " + buttons[0].name + ": " + playerUpgradeManager.resistanceCost + " Scrap Currency";
        } 
    }
    
    public void PlayerUpgradeDepth()
    {
        if (Economy.Scrap >= playerUpgradeManager.depthCost && playerUpgradeManager.currentDepthIndex < playerUpgradeManager.depthResistance.Count)
        {
            playerUpgradeManager.GiveDepthResistanceUpgrade();
            Economy.TakeScrap(playerUpgradeManager.depthCost);
            if (playerUpgradeManager.currentDepthIndex == playerUpgradeManager.depthResistance.Count)
            {
                buttons[1].SetActive(false);
                return;
            }
            TextMeshProUGUI tmp = buttons[1].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = playerUpgradeManager.depthResistance[playerUpgradeManager.currentDepthIndex - 1] + " " + buttons[1].name + ": " + playerUpgradeManager.depthCost + " Scrap Currency";
        }
    }
    
    public void PlayerUpgradeFloatie()
    {
        if (Economy.Scrap >= playerUpgradeManager.floatieCost && !playerUpgradeManager.GetComponent<AndrewMovement>().canSwim)
        {
            playerUpgradeManager.GiveSwimAbilityUpgrade();
            Economy.TakeScrap(playerUpgradeManager.floatieCost);
            buttons[2].SetActive(false);
        }
    }
    
    public void PlayerUpgradeInventory()
    {
        if (Economy.Scrap >= playerUpgradeManager.inventoryCost && playerUpgradeManager.currentInventoryCapacityIndex < playerUpgradeManager.inventoryCapacity.Count)
        {
            playerUpgradeManager.GiveInventoryUpgrade();
            Economy.TakeScrap(playerUpgradeManager.inventoryCost);
            if (playerUpgradeManager.currentInventoryCapacityIndex == playerUpgradeManager.inventoryCapacity.Count)
            {
                buttons[5].SetActive(false);
                return;
            }
            TextMeshProUGUI tmp = buttons[5].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = playerUpgradeManager.inventoryCapacity[playerUpgradeManager.currentInventoryCapacityIndex - 1] + " " + buttons[5].name + ": " + playerUpgradeManager.inventoryCost + " Scrap Currency";
        }
    }
    
    public void PlayerUpgradeSpeed()
    {
        if (Economy.Scrap >= playerUpgradeManager.speedCost && playerUpgradeManager.currentSwimSpeedIndex < playerUpgradeManager.swimSpeed.Count)
        {
            playerUpgradeManager.GiveSwimSpeedUpgrade();
            Economy.TakeScrap(playerUpgradeManager.speedCost);
            if (playerUpgradeManager.currentSwimSpeedIndex == playerUpgradeManager.swimSpeed.Count)
            {
                buttons[4].SetActive(false);
                return;
            }
            TextMeshProUGUI tmp = buttons[4].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = playerUpgradeManager.swimSpeed[playerUpgradeManager.currentSwimSpeedIndex - 1] + " " + buttons[4].name + ": " + playerUpgradeManager.speedCost + " Scrap Currency";
        }
    }
    
    public void PlayerUpgradeGoggles()
    {
        if (Economy.Scrap >= playerUpgradeManager.goggleCost && !playerUpgradeManager.hasGoggles)
        {
            playerUpgradeManager.GiveGoggleUpgrade();
            Economy.TakeScrap(playerUpgradeManager.goggleCost);
            buttons[3].SetActive(false);
        }
    }
    
    public void VehicleUpgradeTurn()
    {
        if (Economy.Scrap >= boatUpgradeManager.turnCost && boatUpgradeManager.currentTurnEffectivenessIndex < boatUpgradeManager.turnEffectivenessList.Count)
        {
            boatUpgradeManager.GiveTurnEffectivenessUpgrade();
            Economy.TakeScrap(boatUpgradeManager.turnCost);
            if (boatUpgradeManager.currentTurnEffectivenessIndex == boatUpgradeManager.turnEffectivenessList.Count)
            {
                buttons[6].SetActive(false);
                return;
            }
            TextMeshProUGUI tmp = buttons[6].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = boatUpgradeManager.turnEffectivenessList[boatUpgradeManager.currentTurnEffectivenessIndex - 1] + " " + buttons[6].name + ": " + boatUpgradeManager.turnCost + " Scrap Currency";
        }
    }
    
    public void VehicleUpgradeSail()
    {
        if (Economy.Scrap >= boatUpgradeManager.sailCost && boatUpgradeManager.currentSailSpeedAndSizeIndex < boatUpgradeManager.sailSpeedAndSizeList.Count)
        {
            boatUpgradeManager.GiveSailSpeedAndSizeUpgrade();
            Economy.TakeScrap(boatUpgradeManager.sailCost);
            if (boatUpgradeManager.currentSailSpeedAndSizeIndex == boatUpgradeManager.sailSpeedAndSizeList.Count)
            {
                buttons[7].SetActive(false);
                return;
            }
            TextMeshProUGUI tmp = buttons[7].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = boatUpgradeManager.sailSpeedAndSizeList[boatUpgradeManager.currentSailSpeedAndSizeIndex - 1] + " " + buttons[7].name + ": " + boatUpgradeManager.sailCost + " Scrap Currency";
        }
    }
    
    public void VehicleUpgradeMotor()
    {
        if (Economy.Scrap >= boatUpgradeManager.motorCost && !boatUpgradeManager.motorUpgrade)
        {
            boatUpgradeManager.GiveMotorUpgrade();
            Economy.TakeScrap(boatUpgradeManager.motorCost);
            buttons[8].SetActive(false);
        }
    }
    
    public void VehicleUpgradeSubmersible()
    {
        if (Economy.Scrap >= boatUpgradeManager.subCost && boatUpgradeManager.motorUpgrade && !boatUpgradeManager.submersible)
        {
            boatUpgradeManager.GiveSubmersibleUpgrade();
            Economy.TakeScrap(boatUpgradeManager.subCost);
            buttons[11].SetActive(false);
        }
    }
    
    public void VehicleUpgradeWindTerminal()
    {
        if (Economy.Scrap >= boatUpgradeManager.terminalCost && !boatUpgradeManager.windTerminal)
        {
            boatUpgradeManager.GiveOnboardWindTerminalUpgrade();
            Economy.TakeScrap(boatUpgradeManager.terminalCost);
            buttons[9].SetActive(false);
        }
    }
    
    public void VehicleUpgradeFinal()
    {
        if (Economy.Scrap >= boatUpgradeManager.finalCost && boatUpgradeManager.submersible && !boatUpgradeManager.hasFinalUpgrade)
        {
            boatUpgradeManager.GiveFinalBoatUpgrade();
            Economy.TakeScrap(boatUpgradeManager.finalCost);
            buttons[10].SetActive(false);
        }
    }
}
