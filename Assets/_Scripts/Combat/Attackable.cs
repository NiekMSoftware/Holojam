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
        private bool isPlayerCharacter;
        [SerializeField]
        private AttackableFaction myFaction;
        [SerializeField]
        private int CurrentHP = 1;
        [SerializeField]
        private int MaxHP = 1;
        private Rigidbody2D mRigidBody;
        private Core corePlayer;
        void Start()
        {
            corePlayer = GetComponent<Core>();
            mRigidBody = GetComponent<Rigidbody2D>();
            SetFaction(myFaction);
        }
        public bool CanAttack(Hitbox hb)
        {
            if (isInvincible) return false;
            if (hb.faction == AttackableFaction.NEUTRAL) return true;
            return hb.faction != myFaction;
        }
        public void SetFaction(AttackableFaction newFaction)
        {
            myFaction = newFaction;
            Hitbox[] allHitboxes = GetComponentsInChildren<Hitbox>();
            foreach(Hitbox hb in allHitboxes)
            {
                hb.faction = newFaction;
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
        public void TakeHit(Hitbox hb)
        {
            CurrentHP = Mathf.Clamp(CurrentHP - hb.damage, 0, MaxHP);
            if (mRigidBody == null) return;
            Vector2 finalKB = new Vector2((hb.transform.position.x - transform.position.x > 0 ? -1 : 1) * hb.KnockbackImpulse.x, hb.KnockbackImpulse.y);
            if (Mathf.Abs(finalKB.y) != 0) mRigidBody.linearVelocityY = 0;
            mRigidBody.AddForce(finalKB, ForceMode2D.Impulse);
            if (CurrentHP == 0)
            {
                corePlayer.PerformAction("die");
            } else
            {
                corePlayer.PerformAction("hurt");
            }
        }
    }
}
