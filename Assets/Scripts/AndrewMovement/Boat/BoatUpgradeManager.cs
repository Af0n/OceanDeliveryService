using UnityEngine;
using System.Collections.Generic;

public class BoatUpgradeManager : MonoBehaviour
{
    public float turnEffectiveness;
    public List<float> turnEffectivenessList = new List<float>();
    private int currentTurnEffectivenessIndex = 0;

    public float sailSpeedAndSize;
    public List<float> sailSpeedAndSizeList = new List<float>();
    private int currentSailSpeedAndSizeIndex = 0;

    public bool motorUpgrade;

    public bool submersible;

    public bool windTerminal;
    public GameObject onboardWindTerminal;

    public bool hasFinalUpgrade;
    
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
        onboardWindTerminal.SetActive(windTerminal);
    }
    
    public void GiveFinalBoatUpgrade()
    {
        hasFinalUpgrade = true;
    }
}
