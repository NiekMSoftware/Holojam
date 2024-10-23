using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class CorruptionActivatePickup : MonoBehaviour
    {
        public CorruptionType corruptionType;
        public bool valueToSetTo = true;
        public bool disableAfterPickup = true;
        private bool disabled = false;
        public SpriteRenderer mSpriteRenderer;
        public ParticleSystem mPartSys;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (disabled) return;

            if (collision.attachedRigidbody.GetComponent<Player>() == null) return;
            CorruptionManager.SetEffectActive(corruptionType, valueToSetTo);
            if (disableAfterPickup)
            {
                mSpriteRenderer.color = new Color(0, 0, 0, 0);
                mPartSys.Stop();
                disabled = true;
            }
        }
    }
}
