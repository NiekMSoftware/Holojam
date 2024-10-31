using UnityEngine;
using HoloJam.Managers;
namespace HoloJam
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float lifeTime;
        [SerializeField]
        private bool destroyOnHit;
        [SerializeField]
        private bool DestroyOnWall;

        private Rigidbody2D mRigidBody;
        [SerializeField]
        private Vector2 initialVelocity;
        [SerializeField]
        private Vector2 velocityChange;
        private Vector2 currentVelocity;
        private float expirationTime;
        private CorruptableObject coObject;
        [SerializeField]
        private string sfxOnImpact;
        [SerializeField]
        private string sfxOnSummon;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            expirationTime = Time.timeSinceLevelLoad + lifeTime;
            mRigidBody = GetComponent<Rigidbody2D>();
            coObject = GetComponent<CorruptableObject>();
            currentVelocity = initialVelocity;
            AudioManager.Instance.Play(sfxOnSummon);
        }
        public void FlipVelocity()
        {
            initialVelocity = new Vector2(-initialVelocity.x, initialVelocity.y);
            currentVelocity = new Vector2(-currentVelocity.x, currentVelocity.y);
        }
        public void SetVelocity(Vector2 newVel)
        {
            initialVelocity = newVel;
            currentVelocity = newVel;
        }
        private void Update()
        {
            if (coObject != null && coObject.Frozen)
            {
                expirationTime += Time.deltaTime;
            }
            if (Time.timeSinceLevelLoad > expirationTime)
            {
                Destroy(gameObject);
            }
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (coObject != null && coObject.Frozen)
            {
                mRigidBody.linearVelocity = Vector2.zero;
            } else
            {
                currentVelocity += velocityChange * Time.deltaTime;
                mRigidBody.linearVelocity = currentVelocity;
            }
        }

        public void OnHitboxHit(Attackable target)
        {
            if (destroyOnHit)
            {
                AudioManager.Instance.Play(sfxOnImpact);
                Destroy(gameObject);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody != null && collision.attachedRigidbody.GetComponent<ButtonSwitch>() != null) Destroy(gameObject);
            if (collision.isTrigger) return;
            if (collision.attachedRigidbody != null && collision.attachedRigidbody.GetComponent<Attackable>() != null) return;

            if (DestroyOnWall)
            {
                AudioManager.Instance.Play(sfxOnImpact);
                Destroy(gameObject);
            }
        }
    }
}
