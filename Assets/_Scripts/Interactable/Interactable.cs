using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public enum InteractableType { ATTACK, DIALOGUE, EXIT, GRAB, MISC }
    public class Interactable : MonoBehaviour
    {
        public int priority = 0;
        public InteractableType interactionType;
        public virtual void OnPerformInteraction(Player p)
        {

        }
        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody == null || collision.attachedRigidbody.GetComponent<Interactor>() == null ||
                collision.GetComponent<Hitbox>() != null) return;

            collision.attachedRigidbody.GetComponent<Interactor>().RegisterInteractable(this);
        }
        public virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.attachedRigidbody == null || collision.attachedRigidbody.GetComponent<Interactor>() == null ||
                collision.GetComponent<Hitbox>() != null) return;
            collision.attachedRigidbody.GetComponent<Interactor>().DeregisterInteractable(this);
        }
    }
}
