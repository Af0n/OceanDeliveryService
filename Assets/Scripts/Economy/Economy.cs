using UnityEngine;

public static class Economy
{
    public static int Scrap = 20;

    public static void ChangeScrap(int amount){
        Scrap += amount;
        Scrap = Mathf.Clamp(Scrap, 0, 999);
        Debug.Log("Scrap: " + Scrap);
    }
    
    public static void TakeScrap(int amount){
        Scrap -= amount;
        Scrap = Mathf.Clamp(Scrap, 0, 999);
        Debug.Log("Scrap: " + Scrap);
    }
}
