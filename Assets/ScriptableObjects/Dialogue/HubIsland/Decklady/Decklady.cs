using UnityEngine;

public class Decklady : Interactable
{
    public Flags flags;
    public DialogueChain IntroChain, WoahChain, ByeChain;

    private void Awake()
    {
        flags.SetAll(false);
    }

    public override void Interact()
    {
        switch (flags.CheckFlag("UsedATMOS"))
        {
            case 0:
                DialogueManager.instance.StartDialogue(IntroChain);
                return;
            case 1:
                // continue
                break;
            default:
                DialogueManager.instance.StartDialogue(IntroChain);
                break;
        }

        switch (flags.CheckFlag("HasWoahed"))
        {
            case 0:
                flags.SetFlag("HasWoahed", true);
                DialogueManager.instance.StartDialogue(WoahChain);
                return;
            case 1:
                // continue
                break;
            default:
                DialogueManager.instance.StartDialogue(WoahChain);
                break;
        }

        DialogueManager.instance.StartDialogue(ByeChain);
    }
}
