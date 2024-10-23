using UnityEngine;

namespace HoloJam
{
    public class CorruptableObject : MonoBehaviour
    {
        [HideInInspector]
        public int NegationFieldsOverlapped = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            CorruptionManager.RegisterObject(this);
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
        public virtual void OnReceiveEffect(CorruptionType effect)
        {
            Debug.Log(gameObject.name + " is being corrupted by: " + effect);
            switch (effect)
            {
                case CorruptionType.GRAVITY:
                    break;
                case CorruptionType.TIMESTOP:
                    break;
                default:
                    break;
            }
        }
    }
}
