using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class COPlayer : CorruptableObject
    {
        [SerializeField]
        private GameObject GlobeObject;
        private Player mPlayer;
        private void Start()
        {
            BaseStart();
            mPlayer = GetComponent<Player>();
        }
        public override void OnReceiveEffect(CorruptionType effect)
        {
            switch (effect)
            {
                case CorruptionType.GRAVITY:
                    OnGravityInvert(!invertedGravity);
                    break;
                case CorruptionType.TIMESTOP:
                    break;
                case CorruptionType.KILL:
                    Kill();
                    break;
                case CorruptionType.GLOBE:
                    Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                    Instantiate(GlobeObject, spawnPos, Quaternion.identity);
                    break;
                case CorruptionType.BIRD:
                    mPlayer.ToggleBird();
                    break;
                default:
                    break;
            }
        }
    }
}
