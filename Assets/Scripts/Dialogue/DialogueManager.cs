using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Unity Set Up")]
    public Transform Canvas;
    public TextMeshProUGUI CharName;
    public TextMeshProUGUI Dialogue;
    [Tooltip("If a text is populated with this string, it will not be changed.")]
    public string CopyStr;

    public DialogueChain testChain;

    private DialogueChain chain;
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
    }

    private void SetChain(DialogueChain c){
        chain = c;
        index = -1;
        Next();
    }

    private void Next(){
        index++;

        if(index >= chain.Dialogues.Length){
            EndDialogue();
            return;
        }

        SetDialogue(chain.Dialogues[index]);
    }

    private void EndDialogue(){
        Canvas.gameObject.SetActive(false);
    }

    private void StartDialogue(DialogueChain d){
        SetChain(d);

        Canvas.gameObject.SetActive(true);
    }
}
