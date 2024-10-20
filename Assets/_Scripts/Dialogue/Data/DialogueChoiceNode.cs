using System.Collections.Generic;
using UnityEngine;

namespace HoloJam.Dialogue.Data
{
    [CreateAssetMenu(fileName = "NewDialogueChoiceNode", menuName = "Dialogue/Choice Node")]
    public class DialogueChoiceNode : DialogueNode
    {
        [System.Serializable]
        public struct Choice
        {
            public string choiceText;
            public DialogueNode nextNode;
        }

        public List<Choice> choices;

        public override void EnterNode()
        {
            DialogueManager.Instance.DisplayChoices(characterName, dialogueText, choices);
        }
    }
}