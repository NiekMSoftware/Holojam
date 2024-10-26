using UnityEngine;
using HoloJam.Characters.Player;
using HoloJam.Characters.Player.Utils;
using System.Collections.Generic;
namespace HoloJam
{
    public class Interactor : MonoBehaviour
    {
        private Player basePlayer;
        private List<Interactable> currentInteractables = new List<Interactable>();
        [SerializeField]
        private Animator interactionAnimator;
        private Interactable highestInteractable;
        private PlayerInput input;
        public bool HandsFree { get { return canInteract; } set { canInteract = value; } }
        private bool canInteract;
        private void Start()
        {
            basePlayer = GetComponent<Player>();
            input = GetComponentInParent<Player>().Input;
            canInteract = true;
        }
        public void RegisterInteractable(Interactable interactable)
        {
            if (currentInteractables.Contains(interactable)) return;
            currentInteractables.Add(interactable);
            UpdateIcons();
        }
        public void DeregisterInteractable(Interactable interactable)
        {
            if (!currentInteractables.Contains(interactable)) return;
            currentInteractables.Remove(interactable);
            UpdateIcons();
        }
        

        // Update is called once per frame
        void Update()
        {
            if (basePlayer.performingAction || !canInteract) return;
            if (input.GetInteractValue() > 0 && highestInteractable != null)
            {
                highestInteractable.OnPerformInteraction(basePlayer);
            }
        }

        private void UpdateIcons()
        {
            int highestInteractablePriority = -1;
            highestInteractable = null;
            foreach(Interactable i in currentInteractables)
            {
                if (i.priority > highestInteractablePriority) { 
                    highestInteractablePriority = i.priority;
                    highestInteractable = i;
                }
            }
            if (highestInteractable == null || !HandsFree)
            {
                interactionAnimator.Play("idle");
                return;
            }
            switch (highestInteractable.interactionType)
            {
                case InteractableType.ATTACK:
                    interactionAnimator.Play("attack");
                    return;
                case InteractableType.DIALOGUE:
                    interactionAnimator.Play("dialogue");
                    return;
                case InteractableType.EXIT:
                    interactionAnimator.Play("exit");
                    return;
                case InteractableType.GRAB:
                    interactionAnimator.Play("grab");
                    return;
                default:
                    interactionAnimator.Play("misc");
                    return;
            }
        }
    }
}