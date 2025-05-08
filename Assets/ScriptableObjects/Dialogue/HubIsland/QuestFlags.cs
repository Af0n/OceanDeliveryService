using UnityEngine;

public class QuestFlags : MonoBehaviour
{
    public Flags quest1;
    public Flags quest2;
    public Flags quest3;
    
    private void Awake()
    {
        quest1.SetAll(false);
        quest2.SetAll(false);
        quest3.SetAll(false);
    }
}
