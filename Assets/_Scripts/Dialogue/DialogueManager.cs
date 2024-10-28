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
            if (player == null) player = FindFirstObjectByType<Player>();
            UIManager.Instance.ShowDialogue(node.characterName, node.dialogueText);
            player.Input.EnableUIControls();
            SetCurrentNode(node);
        }

        public void EndDialogue()
        {
            print("Ending regular dialogue.");
            UIManager.Instance.HideDialogue();
            player.Input.DisableUIControls();
            CurrentNode = null;
        }

        public void EndDialogue(string sceneName)
        {
            print("Ending Dialogue and loading scene!");
            UIManager.Instance.HideDialogue();
            player.Input.DisableUIControls();
            CurrentNode = null;

            // load a new scene
            WorldManager.LoadNewScene(sceneName);
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
            if (PauseManager.IsPaused) return;

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
            else if (CurrentNode is DialogueSceneNode sceneNode)
            {
                if (sceneNode.NextNode != null)
                {
                    OnDialogueLineCompleted();
                    SetCurrentNode(sceneNode.NextNode);
                }
                else
                { 
                    EndDialogue(sceneNode.SceneToLoad);
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
