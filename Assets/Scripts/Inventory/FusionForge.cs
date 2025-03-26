using UnityEngine;

public class FusionForge : MonoBehaviour
{
    public BoatUpgradeManager boatUpgradeManager;
    public PlayerUpgradeManager playerUpgradeManager;
    
    public GameObject playerTab;
    public GameObject vehicleTab;
    
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
        if (Economy.Scrap > playerUpgradeManager.resistanceCost && playerUpgradeManager.currentWaterIndex < playerUpgradeManager.waterResistance.Count)
        {
            playerUpgradeManager.GiveWaterResistanceUpgrade();
            Economy.TakeScrap(playerUpgradeManager.resistanceCost);
        }
    }
    
    public void PlayerUpgradeDepth()
    {
        if (Economy.Scrap > playerUpgradeManager.depthCost && playerUpgradeManager.currentDepthIndex < playerUpgradeManager.depthResistance.Count)
        {
            playerUpgradeManager.GiveDepthResistanceUpgrade();
            Economy.TakeScrap(playerUpgradeManager.depthCost);
        }
    }
    
    public void PlayerUpgradeFloatie()
    {
        if (Economy.Scrap > playerUpgradeManager.floatieCost && !playerUpgradeManager.swimAbilityUpgrade)
        {
            playerUpgradeManager.GiveSwimAbilityUpgrade();
            Economy.TakeScrap(playerUpgradeManager.floatieCost);
        }
    }
    
    public void PlayerUpgradeInventory()
    {
        if (Economy.Scrap > playerUpgradeManager.inventoryCost && playerUpgradeManager.currentInventoryCapacityIndex < playerUpgradeManager.inventoryCapacity.Count)
        {
            playerUpgradeManager.GiveInventoryUpgrade();
            Economy.TakeScrap(playerUpgradeManager.inventoryCost);
        }
    }
    
    public void PlayerUpgradeSpeed()
    {
        if (Economy.Scrap > playerUpgradeManager.speedCost && playerUpgradeManager.currentSwimSpeedIndex < playerUpgradeManager.swimSpeed.Count)
        {
            playerUpgradeManager.GiveSwimSpeedUpgrade();
            Economy.TakeScrap(playerUpgradeManager.speedCost);
        }
    }
    
    public void PlayerUpgradeGoggles()
    {
        if (Economy.Scrap > playerUpgradeManager.goggleCost && !playerUpgradeManager.hasGoggles)
        {
            playerUpgradeManager.GiveGoggleUpgrade();
            Economy.TakeScrap(playerUpgradeManager.goggleCost);
        }
    }
    
    public void VehicleUpgradeTurn()
    {
        if (Economy.Scrap > boatUpgradeManager.turnCost && boatUpgradeManager.currentTurnEffectivenessIndex < boatUpgradeManager.turnEffectivenessList.Count)
        {
            boatUpgradeManager.GiveTurnEffectivenessUpgrade();
            Economy.TakeScrap(boatUpgradeManager.turnCost);
        }
    }
    
    public void VehicleUpgradeSail()
    {
        if (Economy.Scrap > boatUpgradeManager.sailCost && boatUpgradeManager.currentSailSpeedAndSizeIndex < boatUpgradeManager.sailSpeedAndSizeList.Count)
        {
            boatUpgradeManager.GiveSailSpeedAndSizeUpgrade();
            Economy.TakeScrap(boatUpgradeManager.sailCost);
        }
    }
    
    public void VehicleUpgradeMotor()
    {
        if (Economy.Scrap > boatUpgradeManager.motorCost && !boatUpgradeManager.motorUpgrade)
        {
            boatUpgradeManager.GiveMotorUpgrade();
            Economy.TakeScrap(boatUpgradeManager.motorCost);
        }
    }
    
    public void VehicleUpgradeSubmersible()
    {
        if (Economy.Scrap > boatUpgradeManager.subCost && boatUpgradeManager.motorUpgrade && !boatUpgradeManager.submersible)
        {
            boatUpgradeManager.GiveSubmersibleUpgrade();
            Economy.TakeScrap(boatUpgradeManager.subCost);
        }
    }
    
    public void VehicleUpgradeWindTerminal()
    {
        if (Economy.Scrap > boatUpgradeManager.terminalCost && !boatUpgradeManager.windTerminal)
        {
            boatUpgradeManager.GiveOnboardWindTerminalUpgrade();
            Economy.TakeScrap(boatUpgradeManager.terminalCost);
        }
    }
    
    public void VehicleUpgradeFinal()
    {
        if (Economy.Scrap > boatUpgradeManager.finalCost && boatUpgradeManager.submersible && !boatUpgradeManager.hasFinalUpgrade)
        {
            boatUpgradeManager.GiveFinalBoatUpgrade();
            Economy.TakeScrap(boatUpgradeManager.finalCost);
        }
    }
}
