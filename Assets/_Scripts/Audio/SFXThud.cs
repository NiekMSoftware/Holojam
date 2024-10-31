using UnityEngine;
using HoloJam.Managers;
namespace HoloJam
{
    public class SFXThud : MonoBehaviour
    {
        public float velocityRequirement;
        public string sfxID;
        public bool RequireOnFeet;
        public float minimumDelay = 0.25f;
        private float lastSFXPlayed;

        private CorruptableObject mCorruptable;
        private Rigidbody2D mRigidBody;
        private void Start()
        {
            mCorruptable = GetComponent<CorruptableObject>();
            mRigidBody = GetComponent<Rigidbody2D>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Time.timeSinceLevelLoad - lastSFXPlayed < minimumDelay) return;
            Vector2 vel = mRigidBody.linearVelocity;

            if (RequireOnFeet)
            {
                if (mCorruptable.InvertedGravity ? vel.y > velocityRequirement : vel.y < -velocityRequirement)
                {
                    PlaySFX();
                }
                return;
            } else if (vel.magnitude > velocityRequirement)
            {
                PlaySFX();
            }
        }
        void PlaySFX()
        {
            AudioManager.Instance.Play(sfxID);
            lastSFXPlayed = Time.timeSinceLevelLoad;
        }
    }
}
