public class TestTalker : Interactable
{
    public DialogueChain DialogueChain;
    public override void Interact()
    {
        DialogueManager.instance.StartDialogue(DialogueChain);
    }
}
