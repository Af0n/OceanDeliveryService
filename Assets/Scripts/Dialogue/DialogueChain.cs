using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueChain", menuName = "Scriptable Objects/DialogueChain")]
public class DialogueChain : ScriptableObject
{
    [Serializable]
    public class Dialogue{
        public string CharName;
        [TextArea]
        public string Text;
    }

    public Dialogue[] Dialogues;
}
