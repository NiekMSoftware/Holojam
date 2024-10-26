using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class Grabbable : Interactable
    {
        private Collider2D mCollider;
        private Rigidbody2D mRigidBody;
        [SerializeField]
        private Vector2 throwVelocityUp = new Vector2();
        [SerializeField]
        private Vector2 throwVelocityForward = new Vector2();
        private bool originalColliderIsTrigger;
        public bool thrown;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            interactionType = InteractableType.GRAB;
            mRigidBody = GetComponent<Rigidbody2D>();
            mCollider = GetComponent<Collider2D>();
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
        }
    }
}
