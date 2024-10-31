using UnityEngine;
using HoloJam.Characters.Player;
using HoloJam.Managers;
namespace HoloJam
{
    public enum InteractableType { ATTACK, DIALOGUE, EXIT, GRAB, MISC }
    public class Interactable : MonoBehaviour
    {
        public int priority = 0;
        public InteractableType interactionType;
        public string saveIDOnInteract;
        public bool CanInteractInMidair = true;
        public string sfxOnHighlight;
        public string sfxOnInteract;
        public virtual void OnPerformInteraction(Player p)
        {

        }
        public void InteractPreProcess()
        {
            if (sfxOnInteract != "") AudioManager.Instance.Play(sfxOnInteract);
            if (saveIDOnInteract != "") MemoryManager.SetVariable(saveIDOnInteract);
        }
        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody == null || collision.attachedRigidbody.GetComponent<Interactor>() == null ||
                collision.GetComponent<Hitbox>() != null) return;
            if (sfxOnHighlight != "") AudioManager.Instance.Play(sfxOnHighlight);
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
