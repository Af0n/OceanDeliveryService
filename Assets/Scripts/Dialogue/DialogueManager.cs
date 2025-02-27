using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Unity Set Up")]
    public Transform Canvas;
    public TextMeshProUGUI CharName;
    public TextMeshProUGUI Dialogue;
    public Transform menu;
    [Tooltip("If a text is populated with this string, it will not be changed.")]
    public string CopyStr;

    public DialogueChain testChain;

    private DialogueChain chain;
    private DialogueChain[] menuOptions;
    private int index;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            if(!Canvas.gameObject.activeSelf){
                StartDialogue(testChain);
            }else{
                Next();
            }
        }
    }

    private void SetDialogue(DialogueChain.Dialogue d){
        if(!d.CharName.Equals(CopyStr)){
            CharName.text = d.CharName;
        }

        if(!d.Text.Equals(CopyStr)){
            Dialogue.text = d.Text;
        }

        if(d.HasMenu){
            menuOptions = d.MenuOptions;
            MenuPopulation();
        }
    }

    private void SetChain(DialogueChain c, int i){
        chain = c;
        index = i;
    }

    private void Next(){
        if(chain.Dialogues[index].DoesJump){
            JumpTo(chain.Dialogues[index].JumpChain, chain.Dialogues[index].JumpIndex);
            return;
        }

        index++;

        if(index >= chain.Dialogues.Length){
            EndDialogue();
            return;
        }

        SetDialogue(chain.Dialogues[index]);
    }

    private void JumpTo(DialogueChain c, int i){
        if(i >= c.Dialogues.Length){
            // sanity checking
            i=0;
        }
        SetChain(c, i);
        SetDialogue(chain.Dialogues[index]);
    }

    private void EndDialogue(){
        Canvas.gameObject.SetActive(false);
    }

    private void StartDialogue(DialogueChain d){
        SetChain(d, 0);
        SetDialogue(chain.Dialogues[index]);
        Canvas.gameObject.SetActive(true);
    }

    private void MenuPopulation(){
        int i = 0;
        foreach (DialogueChain chain in menuOptions)
        {
            Transform bttn = menu.GetChild(i);
            bttn.gameObject.SetActive(true);

            Transform bttnText = bttn.GetChild(0);

            TextMeshProUGUI textMesh = bttnText.GetComponent<TextMeshProUGUI>();
            textMesh.text = chain.Label;

            i++;
        }
    }

    public void MenuSelection(int i){
        SetChain(menuOptions[i], 0);
        SetDialogue(chain.Dialogues[index]);
        for(int c=0; c<menu.childCount; c++)
        {
            menu.GetChild(c).gameObject.SetActive(false);
        }
    }
}
