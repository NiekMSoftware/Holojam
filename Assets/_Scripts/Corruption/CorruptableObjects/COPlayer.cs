using UnityEngine;

namespace HoloJam
{
    public class COPlayer : CorruptableObject
    {
        public override void OnReceiveEffect(CorruptionType effect)
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
