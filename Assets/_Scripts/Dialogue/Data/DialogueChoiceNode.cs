using System.Collections.Generic;
using UnityEngine;

namespace HoloJam.Dialogue.Data
{
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
            // dialogue manager
        }
    }
}
