using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class SwitchInteractable : Interactable
    {
        public delegate void ToggleEvent(Player p);
        public ToggleEvent toggleEvent;
        public override void OnPerformInteraction(Player p)
        {
            if (toggleEvent != null) toggleEvent(p);
        }
    }
}
