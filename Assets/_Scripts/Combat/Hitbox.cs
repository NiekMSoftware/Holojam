using UnityEngine;
using System.Collections.Generic;
namespace HoloJam
{
    public class Hitbox : MonoBehaviour
    {
        public AttackableFaction faction;
        public int damage;
        public Vector2 KnockbackImpulse;
        private const float REFRESH_RATE = 0.3f;
        private Dictionary<Attackable, float> lastTimeHit = new Dictionary<Attackable, float>();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Attackable attackable = collision.attachedRigidbody.GetComponent<Attackable>();
            if (attackable  == null) return;
            if (!attackable.CanAttack(this)) return;
            if (lastTimeHit.ContainsKey(attackable) && Time.timeSinceLevelLoad - lastTimeHit[attackable] < REFRESH_RATE) return;
            lastTimeHit[attackable] = Time.timeSinceLevelLoad;
            attackable.TakeHit(this);
        }
    }
}
