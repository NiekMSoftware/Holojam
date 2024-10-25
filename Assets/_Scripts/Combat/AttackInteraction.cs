using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class AttackInteraction : Interactable
    {
        private string attackAnim = "attack";
        private void Start()
        {
            interactionType = InteractableType.ATTACK;
        }
        public override void OnPerformInteraction(Player p)
        {
            p.PerformAction(attackAnim);
        }
    }
}
