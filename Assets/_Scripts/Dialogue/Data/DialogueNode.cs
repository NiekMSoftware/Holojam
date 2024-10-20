using UnityEngine;

namespace HoloJam.Dialogue.Data
{
    public abstract class DialogueNode : ScriptableObject
    {
        public string characterName;
        public string dialogueText;

        public abstract void EnterNode();
    }
}
