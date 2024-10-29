using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class Grabbable : Interactable
    {
        private Collider2D mCollider;
        public Rigidbody2D mRigidBody;
        [SerializeField]
        private Vector2 throwVelocityUp = new Vector2();
        [SerializeField]
        private Vector2 throwVelocityForward = new Vector2();
        [SerializeField]
        private Collider2D extraColliderToConvertToTrigger;
        private bool originalColliderIsTrigger;
        public bool thrown;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            interactionType = InteractableType.GRAB;
            mCollider = GetComponent<Collider2D>();
            mRigidBody = mCollider.attachedRigidbody;
            originalColliderIsTrigger = mCollider.isTrigger;
        }
        public override void OnPerformInteraction(Player p)
        {
            Grabber g = p.GetComponent<Grabber>();
            if (g == null) return;
            g.SetGrabObject(this);
        }
        public void OnGrabbed()
        {
            mCollider.isTrigger = true;
            if (extraColliderToConvertToTrigger != null)
            {
                extraColliderToConvertToTrigger.isTrigger = true;
            }
            mRigidBody.angularVelocity = 0;
        }
        public void ThrowUp(bool isFacingLeft)
        {
            mRigidBody.linearVelocity = Vector2.zero;
            mRigidBody.AddForce(new Vector2((isFacingLeft ? -1 : 1) * throwVelocityUp.x, throwVelocityUp.y), ForceMode2D.Impulse);
            OnRelease();
        }
        public void ThrowForward(bool isFacingLeft)
        {
            mRigidBody.linearVelocity = Vector2.zero;
            mRigidBody.AddForce(new Vector2((isFacingLeft ? -1 : 1) * throwVelocityForward.x, throwVelocityForward.y),ForceMode2D.Impulse);
            OnRelease();
        }
        public void Drop()
        {
            OnRelease();
        }
        private void OnRelease()
        {
            thrown = true;
            mCollider.isTrigger = originalColliderIsTrigger;
            if (extraColliderToConvertToTrigger != null)
            {
                extraColliderToConvertToTrigger.isTrigger = false;
            }
        }
    }
}
