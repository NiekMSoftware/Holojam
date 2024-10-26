using UnityEngine;
using HoloJam.Characters;

namespace HoloJam
{
    public enum AttackableFaction { FRIENDLY, ENEMY, NEUTRAL} //Neutral can be attacked by both friendly and enemy
    public class Attackable : MonoBehaviour
    {
        [SerializeField]
        private bool isInvincible;
        [SerializeField]
        private bool canBeHurt = true;
        [SerializeField]
        private bool isPlayerCharacter;
        public AttackableFaction MyFaction { get { return myFaction; } }
        [SerializeField]
        private AttackableFaction myFaction;
        [SerializeField]
        private int CurrentHP = 1;
        [SerializeField]
        private int MaxHP = 1;
        [SerializeField]
        private ParticleSystem partSys;
        private Rigidbody2D mRigidBody;
        private Core corePlayer;
        public delegate void OnHitEvent(Hitbox hb);
        public OnHitEvent hitEvent;
        void Start()
        {
            if (partSys != null) { partSys.Stop(); }
            corePlayer = GetComponent<Core>();
            mRigidBody = GetComponent<Rigidbody2D>();
            SetFaction(myFaction);
        }
        public bool CanAttack(Hitbox hb)
        {
            if (isInvincible) return false;
            if (hb.Faction == AttackableFaction.NEUTRAL) return true;
            return hb.Faction != myFaction;
        }
        public void SetFaction(AttackableFaction newFaction)
        {
            myFaction = newFaction;
            Hitbox[] allHitboxes = GetComponentsInChildren<Hitbox>();
            foreach(Hitbox hb in allHitboxes)
            {
                hb.SetParentAttackable(this);
            }
        }
        public void OnDeathAnimationEnd()
        {
            if (isPlayerCharacter)
            {
                WorldManager.ReturnToHomeScene();
            } else
            {
                Destroy(gameObject);
            }
        }
        public void TakeDamage(int damage)
        {
            CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
            if (CurrentHP == 0)
            {
                corePlayer?.PerformAction("die");
            }
            else
            {
                corePlayer?.PerformAction("hurt");
            }
            if (partSys != null) { partSys.Play(); }
        }
        public void TakeHit(Hitbox hb)
        {
            CurrentHP = Mathf.Clamp(CurrentHP - hb.damage, 0, MaxHP);
            if (mRigidBody == null) return;
            Vector3 theirPos = hb.ParentAttackable != null ? hb.ParentAttackable.transform.position : hb.transform.position;
            Vector2 finalKB = new Vector2((theirPos.x - transform.position.x > 0 ? -1 : 1) * hb.KnockbackImpulse.x, hb.KnockbackImpulse.y);
            if (Mathf.Abs(finalKB.y) != 0) mRigidBody.linearVelocityY = 0;
            mRigidBody.AddForce(finalKB, ForceMode2D.Impulse);
            if (hitEvent != null) hitEvent(hb);
            if (CurrentHP == 0)
            {
                corePlayer?.PerformAction("die");
            } else
            {
                corePlayer?.PerformAction("hurt");
            }
            if (partSys != null) { partSys.Play(); }
        }
    }
}
