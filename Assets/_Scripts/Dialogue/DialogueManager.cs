using HoloJam.Dialogue.Data;
using HoloJam.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoloJam.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }
        public event Action OnDialogueLineComplete;

        public DialogueNode CurrentNode;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        public void StartDialogue(DialogueNode node)
        {
            SetCurrentNode(node);
        }

        private void EndDialogue()
        {
            UIManager.Instance.HideDialogue();
        }

        public void SetCurrentNode(DialogueNode node)
        {
            if (node == null)
            {
                // end if none are present
                EndDialogue();
                return;
            }

            CurrentNode = node;
            CurrentNode.EnterNode();
        }

        public void OnDialogueLineCompleted()
        {
            OnDialogueLineComplete?.Invoke();
        }

        #region UI Methods

        public void ContinueDialogue()
        {
            if (CurrentNode is DialogueLineNode lineNode)
            {
                if (lineNode.NextNode == null)
                {
                    EndDialogue();
                }
                else
                    OnDialogueLineCompleted();
            }
        }

        public void ChooseOption(DialogueChoiceNode.Choice choice)
        {
            SetCurrentNode(choice.nextNode);
        }

        public void DisplayDialogue(string characterName, string text)
        {
            UIManager.Instance.ShowDialogue(characterName, text);
        }

        public void DisplayChoices(string characterName, string text, List<DialogueChoiceNode.Choice> choices)
        {
            UIManager.Instance.ShowChoices(characterName, text, choices);
        }

        #endregion
    }
}
