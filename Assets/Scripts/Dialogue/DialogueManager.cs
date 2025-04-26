using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("Unity Set Up")]
    public Transform Canvas;
    public TextMeshProUGUI CharName;
    public TextMeshProUGUI Dialogue;
    public Transform menu;
    [Tooltip("If a text is populated with this string, it will not update what is currently in the box.")]
    public string CopyStr;

    // public DialogueChain testChain;
    public static DialogueManager instance;

    private DialogueChain chain;
    private DialogueChain[] menuOptions;
    private int index;
    private bool menuWait;

    private InputSystem_Actions actions;
    private InputAction advance;
    private PlayerManager playerMan;

    void Awake()
    {
        actions = new InputSystem_Actions();
        instance = this;
    }

    void Start()
    {
        playerMan = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
    }

    private void SetDialogue(DialogueChain.Dialogue d){
        if(!d.CharName.Equals(CopyStr)){
            CharName.text = d.CharName;
        }

        if(!d.Text.Equals(CopyStr)){
            Dialogue.text = d.Text;
        }

        if(d.HasMenu){
            menuWait = true;
            menuOptions = d.MenuOptions;
            MenuPopulation();
        }
    }

    private void SetChain(DialogueChain c, int i){
        chain = c;
        index = i;
    }

    private void Next(InputAction.CallbackContext context){
        // since i cant disable this script, just putting in a check for actual dialogue
        if(!Canvas.gameObject.activeSelf){
            return;
        }

        // disable nexting while waiting for menu
        if(menuWait){
            return;
        }
        
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
        playerMan.SetAll(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartDialogue(DialogueChain d){
        playerMan.SetAll(false);
        SetChain(d, 0);
        SetDialogue(chain.Dialogues[index]);
        Canvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
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
        // SetChain(menuOptions[i], 0);
        // SetDialogue(chain.Dialogues[index]);
        
        var selected = menuOptions[i];
        
        if (selected.Label == "Yes lets get out of here." || selected.Label == "I would like to stay for a little bit."){
            CallFunction(selected.Label);
        }
        else {
            SetChain(menuOptions[i], 0);
            SetDialogue(chain.Dialogues[index]);
        }
        
        for(int c=0; c<menu.childCount; c++)
        {
            menu.GetChild(c).gameObject.SetActive(false);
        }
        menuWait = false;
    }

    private void OnEnable() {
        advance = actions.UI.Advance;
        advance.Enable();
        advance.performed += Next;
    }

    void OnDisable()
    {
        advance.Disable();
    }
    
    
    private void CallFunction(string functionName)
    {
        if (functionName == "I would like to stay for a little bit.")
        {
            EndDialogue();
        }
        else if (functionName == "Yes lets get out of here.")
        {
            FindObjectOfType<Creator>().CompleteGame(); 
            EndDialogue();
        }
        else
        {
            Debug.LogWarning($"No function found for {functionName}");
        }
    }
}
