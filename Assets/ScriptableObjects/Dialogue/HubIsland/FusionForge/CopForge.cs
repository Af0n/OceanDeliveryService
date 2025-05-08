using UnityEngine;

public class CopForge : Interactable
{
    public DialogueChain IntroChain;
    public override void Interact()
    {
        DialogueManager.instance.StartDialogue(IntroChain);
    }
}
