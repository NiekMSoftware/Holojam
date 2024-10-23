using UnityEngine;
using HoloJam.Characters.Player;

namespace HoloJam
{
    public class CorruptionChargePickup : MonoBehaviour
    {
        public CorruptionType corruptionType;
        public int chargeDelta = 1;
        public bool disableAfterPickup = true;

        private bool disabled = false;
        public SpriteRenderer mSpriteRenderer;
        public ParticleSystem mPartSys;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (disabled) return;

            if (collision.attachedRigidbody.GetComponent<Player>() == null) return;
            bool spellsmodified = CorruptionManager.ModifyCharges(corruptionType, chargeDelta);
            if (spellsmodified && disableAfterPickup)
            {
                mSpriteRenderer.color = new Color(0, 0, 0, 0);
                mPartSys.Stop();
                disabled = true;
            }
        }
    }
}
