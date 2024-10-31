using HoloJam.Characters.Player;
using HoloJam.Characters.Player.Utils;
using HoloJam.Dialogue.Data;
using UnityEngine;
namespace HoloJam.Dialogue
{
    public class DialogueTrigger : Interactable
    {
        public DialogueNode StartingNode;
        public bool disableMovement;
        public bool disableJump;
        private PlayerInput input;
        private void Start()
        {
            interactionType = InteractableType.DIALOGUE;
        }
        public override void OnPerformInteraction(Player p)
        {
            DialogueManager.Instance.RegisterDialogueEndEvent(DialogueEndSaveID);
            DialogueManager.Instance.StartDialogue(StartingNode, disableMovement, disableJump);
        }
        void DialogueEndSaveID()
        {
            
        }
        public override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);

            if (collision.CompareTag("Player"))
            {
                DialogueManager.Instance.EndDialogue();
            }
        }
    }
}