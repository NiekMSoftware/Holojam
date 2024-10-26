using HoloJam.Characters.Player;
using HoloJam.Characters.Player.Utils;
using HoloJam.Dialogue.Data;
using UnityEngine;
namespace HoloJam.Dialogue
{
    public class DialogueTrigger : Interactable
    {
        public DialogueNode StartingNode;
        private void Start()
        {
            interactionType = InteractableType.DIALOGUE;
        }
        public override void OnPerformInteraction(Player p)
        {
            DialogueManager.Instance.StartDialogue(StartingNode);
        }
    }
}