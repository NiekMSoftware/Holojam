using UnityEngine;

namespace HoloJam.Dialogue.Data
{
    [CreateAssetMenu(fileName = "Dialogue Scene Node", menuName = "Dialogue/Scene Node")]
    public class DialogueSceneNode : DialogueNode
    {
        public DialogueNode NextNode;
        [Space]

        public string SceneToLoad;

        public override void EnterNode()
        {
            DialogueManager.Instance.DisplayDialogue(characterName, dialogueText);
            DialogueManager.Instance.OnDialogueLineComplete += ProceedToNextNode;
        }

        private void ProceedToNextNode()
        {
            DialogueManager.Instance.OnDialogueLineComplete -= ProceedToNextNode;
            if (NextNode != null) DialogueManager.Instance.SetCurrentNode(NextNode);
        }
    }
}
