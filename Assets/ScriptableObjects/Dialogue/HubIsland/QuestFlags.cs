using UnityEngine;

public class QuestFlags : MonoBehaviour
{
    public QuestData quest1;
    public QuestData quest2;
    public QuestData quest3;
    public QuestData mainGame;
    
    private void Awake()
    {
        quest1.isCompleted = false;
        quest2.isCompleted = false;
        quest3.isCompleted = false;
        mainGame.isCompleted = false;
    }
}
