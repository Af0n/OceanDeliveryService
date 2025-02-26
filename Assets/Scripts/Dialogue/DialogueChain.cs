using System;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueChain", menuName = "Scriptable Objects/DialogueChain")]
public class DialogueChain : ScriptableObject
{
    [Serializable]
    public class Dialogue{
        [Header("Displayed")]
        public string CharName;
        [TextArea]
        public string Text;

        [Header("Menu Settings")]
        public bool HasMenu;
        public DialogueChain[] MenuOptions;

        [Header("Jump Settings")]
        public bool DoesJump;
        public DialogueChain JumpChain;
    }

    [Tooltip("Used for populating menus")]
    public string Label;
    public Dialogue[] Dialogues;
}
