using UnityEngine;
using UnityEngine.Rendering;

public class Creator : Interactable
{
    public Flags flags;
    public DialogueChain IntroChain, ReturnChain, ByeChain;
    public int maxQuests;
    private int currentQuest;
    public SceneChanger sceneChanger;

    private void Awake()
    {
        flags.SetAll(false);
    }

    public void CompleteQuest()
    {
        currentQuest++;
        if (currentQuest == maxQuests)
        {
            flags.SetFlag("FinishedPackages", true);
        }
    }

    public void CompleteGame()
    {
        Debug.Log("Game is over now. Sending to the end scene");
        sceneChanger.ChangeScene(2);
    }

    public override void Interact()
    {
        if (flags.CheckFlag("FinishedPackages") == 1)
        {
            DialogueManager.instance.StartDialogue(ByeChain);
            return;
        }

        if (flags.CheckFlag("HasTalked") == 0)
        {
            DialogueManager.instance.StartDialogue(IntroChain);
            flags.SetFlag("HasTalked", true);
            return;
        }
    
        DialogueManager.instance.StartDialogue(ReturnChain);
    }
}
