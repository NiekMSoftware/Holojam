using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HoloJam.Managers;
namespace HoloJam
{
    public class CorruptableObject : MonoBehaviour
    {
        [SerializeField]
        private bool destroyOnLight;
        [Header("Timestop effects")]
        public bool rigidBodyAffectedByTimestop = true;
        public bool hitboxDisabledByTimestop = true;
        public bool animatorAffectedByTimestop = true;
        public bool isBookwyrm = false;
        public bool Frozen { get { return frozen; } }
        private bool frozen;
        private Animator mAnimator;
        private Rigidbody2D mRigidBody;
        [Header("Gravity effects")]
        public bool affectedByGravity = true;
        public bool flipTransform = false;
        public bool InvertedGravity { get { return invertedGravity; } }
        protected bool invertedGravity;
        [Header("Kill All effects")]
        public bool reactToKill = true;
        private Attackable mAttackable;
        private CorruptionKillAnimator mCorruptionKill;
        [HideInInspector]
        public int NegationFieldsOverlapped = 0;
        private List<Hitbox> mHitboxes = new List<Hitbox>();
        [SerializeField]
        private List<string> destroyOnLightSFX = new List<string>();
        [SerializeField]
        private bool playPlantDeathSFX;
        private bool sfxPlayed = false;
        private ParticleSystem pSys;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            BaseStart();
        }
        private void Update()
        {
            if (destroyOnLight && NegationFieldsOverlapped > 0)
            {
                if (isBookwyrm == true)
                {
                    mAnimator.SetFloat("Frozen", 1);
                }
                mAnimator.Play("die");
                if (!sfxPlayed)
                {
                    sfxPlayed = true;
                    AudioManager.Instance.Play(destroyOnLightSFX[Random.Range(0, destroyOnLightSFX.Count - 1)]);
                }
            }
        }
        protected void BaseStart()
        {
            CorruptionManager.RegisterObject(this);
            mAnimator = GetComponent<Animator>();
            mAttackable = GetComponent<Attackable>();
            mCorruptionKill = GetComponent<CorruptionKillAnimator>();
            mRigidBody = GetComponent<Rigidbody2D>();
            mHitboxes = new List<Hitbox>(GetComponentsInChildren<Hitbox>());
            pSys = GetComponent<ParticleSystem>();
            
        }
        private void OnDestroy()
        {
            CorruptionManager.DeregisterObject(this);
        }
        public void ReceiveEffect(CorruptionType effect)
        {
            if (NegationFieldsOverlapped > 0) return;
            OnReceiveEffect(effect);
        }
        public virtual void OnTimeFreeze(bool newFrozen)
        {
            frozen = newFrozen;
            if (animatorAffectedByTimestop && mAnimator != null)
            {
                if (isBookwyrm == true)
                {
                    mAnimator.SetFloat("Frozen", 0);
                } else
                {
                    mAnimator.speed = newFrozen ? 0 : 1;
                }
                
            }
            if (rigidBodyAffectedByTimestop && mRigidBody != null)
            {
                mRigidBody.linearVelocity = Vector2.zero;
                mRigidBody.angularVelocity = 0;
                mRigidBody.bodyType = newFrozen ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
                mRigidBody.gravityScale = newFrozen ? 0 : (invertedGravity ? -1 : 1);
            }
            if (hitboxDisabledByTimestop)
            {
                mHitboxes.ForEach(hb => hb.enabled = !newFrozen);
            }
        }
        public virtual void OnGravityInvert(bool newInvert)
        {
            if (mRigidBody != null && invertedGravity != newInvert)
            {
                mRigidBody.gravityScale *= -1;
            }
            if (flipTransform && invertedGravity != newInvert)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
            invertedGravity = newInvert;
        }
        public virtual void Kill()
        {
            if (!reactToKill) return;
            if (playPlantDeathSFX) CorruptionManager.PlayPlantDeathSound();
            if (mAttackable != null)
            {
                mAnimator.speed = 1;
                mAttackable.TakeDamage(100);
            } else if (mCorruptionKill != null)
            {
                if (mAnimator != null)
                {
                    mAnimator.speed = 1;
                }
                mCorruptionKill.StartKill();
            } else
            {
                Destroy(gameObject);
            }
        }
        public virtual void OnReceiveEffect(CorruptionType effect)
        {
            switch (effect)
            {
                case CorruptionType.GRAVITY:
                    OnGravityInvert(!invertedGravity);
                    break;
                case CorruptionType.TIMESTOP:
                    OnTimeFreeze(!frozen);
                    break;
                case CorruptionType.KILL:
                    Kill();
                    break;
                default:
                    break;
            }
        }
    }
}
