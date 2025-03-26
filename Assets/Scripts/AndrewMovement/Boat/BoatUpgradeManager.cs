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
        }
    }

    public void GiveSailSpeedAndSizeUpgrade()
    {
        if (currentSailSpeedAndSizeIndex < sailSpeedAndSizeList.Count)
        {
            sailSpeedAndSize = sailSpeedAndSizeList [currentSailSpeedAndSizeIndex];
            currentSailSpeedAndSizeIndex++;
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
}
