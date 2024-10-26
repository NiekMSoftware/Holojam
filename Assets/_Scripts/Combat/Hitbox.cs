using UnityEngine;
using System.Collections.Generic;
namespace HoloJam
{
    public class Hitbox : MonoBehaviour
    {
        public AttackableFaction Faction { get { return faction; } }
        [SerializeField]
        private AttackableFaction faction;
        public int damage;
        public Vector2 KnockbackImpulse;
        private const float REFRESH_RATE = 0.2f;
        private Dictionary<Attackable, float> lastTimeHit = new Dictionary<Attackable, float>();
        public Attackable ParentAttackable { get { return parentAttackable; } }
        private Attackable parentAttackable;
        private Projectile proj;
        private void Start()
        {
            proj = GetComponent<Projectile>();
        }
        public void SetParentAttackable(Attackable parentAttackable)
        {
            this.parentAttackable = parentAttackable;
            faction = parentAttackable.MyFaction;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!enabled) return;
            if (collision.attachedRigidbody == null) return;
            if (collision.GetComponent<Hitbox>() != null) return;
            Attackable attackable = collision.attachedRigidbody.GetComponent<Attackable>();
            if (attackable  == null) return;
            if (attackable == parentAttackable) return;
            if (!attackable.CanAttack(this)) return;
            if (lastTimeHit.ContainsKey(attackable) && Time.timeSinceLevelLoad - lastTimeHit[attackable] < REFRESH_RATE) return;
            lastTimeHit[attackable] = Time.timeSinceLevelLoad;
            attackable.TakeHit(this);
            if (proj != null) proj.OnHitboxHit(attackable);
        }
    }
}
