using HoloJam.Characters.Player;
using HoloJam.Characters.Player.Utils;
using HoloJam.Dialogue.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace HoloJam.Dialogue
{
    public class DialogueTrigger : Interactable
    {
        public string customSaveID = "";
        public DialogueNode StartingNode;
        public bool disableMovement;
        public bool disableJump;
        private PlayerInput input;
        public bool skipOnSecondInteraction;
        public bool triggerReturnToHub;
        public bool oneUsePerWorld = false;
        private bool used = false;
        private void Start()
        {
            interactionType = InteractableType.DIALOGUE;
        }
        private string GetSaveID()
        {
            if (customSaveID != "")
            {
                return customSaveID;
            }
            return SceneManager.GetActiveScene().name;
        }
        public override void OnPerformInteraction(Player p)
        {
            if (oneUsePerWorld && used)
            {
                return;
            }
            if (skipOnSecondInteraction && MemoryManager.HasVariable(GetSaveID()))
            {
                if (triggerReturnToHub)
                {
                    MemoryManager.SetVariable(SceneManager.GetActiveScene().name);
                    WorldManager.ReturnToHomeScene();
                }
                return;
            }
            used = true;
            DialogueManager.Instance.RegisterDialogueEndEvent(DialogueEndSaveID);
            DialogueManager.Instance.StartDialogue(StartingNode, disableMovement, disableJump);
        }
        void DialogueEndSaveID()
        {
            MemoryManager.SetVariable(GetSaveID());
            if (triggerReturnToHub) 
            {
                MemoryManager.SetVariable(SceneManager.GetActiveScene().name);
                WorldManager.ReturnToHomeScene();
            }
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