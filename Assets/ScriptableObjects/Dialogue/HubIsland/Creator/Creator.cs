using UnityEngine;

public class Creator : Interactable
{
    public Flags flags;
    public DialogueChain IntroChain, ReturnChain, ByeChain;
    public int maxQuests;
    private int currentQuest;

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

    public override void Interact()
    {
        switch (flags.CheckFlag("HasTalked"))
        {
            case 0:
                break;
            case 1:
                // continue
                DialogueManager.instance.StartDialogue(ReturnChain);
                return;
            default:
                DialogueManager.instance.StartDialogue(ReturnChain);
                break;
        }
        
        switch (flags.CheckFlag("FinishedPackages"))
        {
            case 0:
                DialogueManager.instance.StartDialogue(IntroChain);
                flags.SetFlag("HasTalked", true);
                return;
            case 1:
                // continue
                break;
            default:
                DialogueManager.instance.StartDialogue(IntroChain);
                flags.SetFlag("HasTalked", true);
                break;
        }
        
        

        DialogueManager.instance.StartDialogue(ByeChain);
    }
}
