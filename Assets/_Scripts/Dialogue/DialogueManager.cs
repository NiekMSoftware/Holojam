using HoloJam.Characters.Player;
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

        [SerializeField] private Player player;
        public DialogueNode CurrentNode;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            if (player == null)
                player = FindFirstObjectByType<Player>();
        }

        public void StartDialogue(DialogueNode node)
        {
            UIManager.Instance.ShowDialogue(node.characterName, node.dialogueText);
            player.Input.SwitchToUIControls();
            SetCurrentNode(node);
        }

        private void EndDialogue()
        {
            UIManager.Instance.HideDialogue();
            player.Input.SwitchToPlayerControls();
            CurrentNode = null;
        }

        public void SetCurrentNode(DialogueNode node)
        {
            if (node != null)
            {
                CurrentNode = node;
                CurrentNode.EnterNode();
            }
        }

        public void OnDialogueLineCompleted()
        {
            OnDialogueLineComplete?.Invoke();
        }

        public void ContinueDialogue()
        {
            if (CurrentNode is DialogueLineNode lineNode)
            {
                if (lineNode.NextNode != null)
                {
                    OnDialogueLineCompleted();
                    SetCurrentNode(lineNode.NextNode);
                }
                else
                { 
                    EndDialogue();
                }
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
    }
}
