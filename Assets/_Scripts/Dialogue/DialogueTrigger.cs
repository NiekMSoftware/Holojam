using HoloJam.Characters.Player;
using HoloJam.Characters.Player.Utils;
using HoloJam.Dialogue.Data;
using UnityEngine;

namespace HoloJam.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        public DialogueNode StartingNode;
        private PlayerInput input;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                // grab the input component once
                if (input == null) input = collision.GetComponentInParent<Player>().Input;

                if (input.GetInteractValue() > 0)
                {
                    print("pressed");
                    DialogueManager.Instance.StartDialogue(StartingNode); 
                }
            }
        }
    }
}