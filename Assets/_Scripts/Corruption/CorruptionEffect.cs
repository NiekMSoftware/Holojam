using UnityEngine;

namespace HoloJam
{
    public enum CorruptionType { GRAVITY, TIMESTOP, ALLIES, SEED, KILL}
    public class CorruptionEffect : MonoBehaviour
    {
        public bool CanBeUsed;
        public int currentCharges = 1;
        public CorruptionType associatedType;
        private void Start()
        {
            CorruptionManager.RegisterEffect(this, associatedType);
        }
        public void ModifyCharges(int delta)
        {
            currentCharges += delta;
        }
        public void ResetCharges()
        {
            currentCharges = 1;
        }
        public bool AttemptUse()
        {
            if (!CanBeUsed) return false;
            if (currentCharges == 0) return false;
            OnEffectUse();
            return true;
        }
        public virtual void OnEffectUse()
        {

        }
    }
}
