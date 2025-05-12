using UnityEngine;
using UnityEngine.Rendering;

public class Wylde : Interactable
{
    public Flags flags;
    public DialogueChain IntroChain, ByeChain;

    private void Awake()
    {
        flags.SetAll(false);
    }

    public override void Interact()
    {
        if (flags.CheckFlag("HasTalked") == 0)
        {
            DialogueManager.instance.StartDialogue(IntroChain);
            flags.SetFlag("HasTalked", true);
            return;
        }
    
        DialogueManager.instance.StartDialogue(ByeChain);
    }
}
