using UnityEngine;

namespace HoloJam.Dialogue.Data
{
    [CreateAssetMenu(fileName = "NewDialogueLineNode", menuName = "Dialogue/Line Node")]
    public class DialogueLineNode : DialogueNode
    {
        public DialogueNode NextNode;

        public override void EnterNode()
        {
            DialogueManager.Instance.DisplayDialogue(characterName, dialogueText);
            DialogueManager.Instance.OnDialogueLineComplete += ProceedToNextNode;
        }

        private void ProceedToNextNode()
        {
            DialogueManager.Instance.OnDialogueLineComplete -= ProceedToNextNode;
            DialogueManager.Instance.SetCurrentNode(NextNode);
        }
    }
}
