using UnityEngine;
using System.Collections.Generic;

public class BoatUpgradeManager : MonoBehaviour
{
    public float turnEffectiveness;
    public List<float> turnEffectivenessList = new List<float>();
    public int currentTurnEffectivenessIndex = 0;
    public int turnCost;

    public float sailSpeedAndSize;
    public List<float> sailSpeedAndSizeList = new List<float>();
    public int currentSailSpeedAndSizeIndex = 0;
    public int sailCost;

    public bool motorUpgrade;
    public int motorCost;

    public bool submersible;
    public int subCost;

    public bool windTerminal;
    public GameObject onboardWindTerminal;
    public int terminalCost;

    public bool hasFinalUpgrade;
    public int finalCost;
    
    public void GiveTurnEffectivenessUpgrade()
    {
        if (currentTurnEffectivenessIndex < turnEffectivenessList.Count)
        {
            turnEffectiveness = turnEffectivenessList[currentTurnEffectivenessIndex];
            currentTurnEffectivenessIndex++;
            turnCost += turnCost / 2;
        }
    }

    public void GiveSailSpeedAndSizeUpgrade()
    {
        if (currentSailSpeedAndSizeIndex < sailSpeedAndSizeList.Count)
        {
            sailSpeedAndSize = sailSpeedAndSizeList [currentSailSpeedAndSizeIndex];
            currentSailSpeedAndSizeIndex++;
            sailCost += sailCost / 2;
        }
    }

    public void GiveMotorUpgrade()
    {
        motorUpgrade = true;
    }

    public void GiveSubmersibleUpgrade()
    {
        submersible = true;
    }

    public void GiveOnboardWindTerminalUpgrade()
    {
        windTerminal = true;
        onboardWindTerminal.SetActive(windTerminal);
    }
    
    public void GiveFinalBoatUpgrade()
    {
        hasFinalUpgrade = true;
    }
    
    public int ListCosts(string name)
    {
        switch (name)
        {
            case "Turn Effectiveness":
                return turnCost;

            case "Sail Size and Speed":
                return sailCost;

            case "Motor":
                return motorCost;

            case "Wind Terminal":
                return terminalCost;
            
            case "Final":
                return finalCost;
            
            case "Submersible":
                return subCost;

            default:
                return -1;
        }
    }

    public float ListCurrent(string name)
    {
        switch (name)
        {
            case "Turn Effectiveness":
                return turnEffectiveness;

            case "Sail Size and Speed":
                return sailSpeedAndSize;

            case "Motor":
                return -1;

            case "Wind Terminal":
                return -1;
            
            case "Final":
                return -1;
            
            case "Submersible":
                return -1;

            default:
                return -1;
        }
    }
}
