using HoloJam.Dialogue.Data;
using HoloJam.Dialogue;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HoloJam.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Text Mesh Pro References")]
        public TMP_Text characterNameText;
        public TMP_Text dialogueText;

        [Space]
        public GameObject choicesPanel;
        public GameObject dialoguePanel;

        [Space]
        public Button choiceButtonPrefab;

        [Header("Dialogue Typing Referenes")]
        public float typingSpeed = 0.05f;
        public float fastTypingSpeed = 0.01f;

        private Coroutine _typingCoroutine;
        private bool _isTypingFast = false;
        private bool _isTypingComplete = false;
        private bool _isChoiceNode = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        public void HideDialogue()
        {
            dialoguePanel.SetActive(false);
            choicesPanel.SetActive(false);

            // Stop the coroutine if it's running
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }
        }

        public void ShowDialogue(string characterName, string text)
        {
            _isChoiceNode = false;
            dialoguePanel.SetActive(true);

            characterNameText.text = characterName;
            dialogueText.text = text;

            // Start typing effect
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }

            _typingCoroutine = StartCoroutine(TypeText(text));
            choicesPanel.SetActive(false);
        }

        public void ShowChoices(string characterName, string text, List<DialogueChoiceNode.Choice> choices)
        {
            _isChoiceNode = true;
            characterNameText.text = characterName;
            dialogueText.text = text;

            // Start typing effect
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }

            _typingCoroutine = StartCoroutine(TypeText(text, choices));
        }

        private IEnumerator TypeText(string text, List<DialogueChoiceNode.Choice> choices = null)
        {
            _isTypingComplete = false;
            dialogueText.text = ""; // Clear the text first

            foreach (char c in text.ToCharArray())
            {
                dialogueText.text += c;

                // If space bar is held down, type fast; otherwise, use normal speed
                float currentTypingSpeed = _isTypingFast ? fastTypingSpeed : typingSpeed;

                yield return new WaitForSeconds(currentTypingSpeed);
            }

            _isTypingComplete = true;
            _typingCoroutine = null; // Reset coroutine reference after typing is complete

            if (_isChoiceNode && choices != null)
            {
                PopulateChoices(choices);
            }
        }

        public void SetTypingSpeed(bool isFast)
        {
            _isTypingFast = isFast;
        }

        public bool IsTypingComplete() => _isTypingComplete;

        private void PopulateChoices(List<DialogueChoiceNode.Choice> choices)
        {
            // clear existing choises
            foreach (Transform child in choicesPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Create new choice buttons
            foreach (var choice in choices)
            {
                Button button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
                button.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
                button.onClick.AddListener(() => OnChoiceSelected(choice));
            }

            choicesPanel.SetActive(true);
        }

        private void OnChoiceSelected(DialogueChoiceNode.Choice choice)
        {
            DialogueManager.Instance.ChooseOption(choice);
        }

        public void FinishTyping(string fullText)
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }

            dialogueText.text = fullText; // Set the full text
            _isTypingComplete = true;
        }
    }
}
