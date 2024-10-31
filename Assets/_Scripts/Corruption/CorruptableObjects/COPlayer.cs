using UnityEngine;
using HoloJam.Characters.Player;
using HoloJam.Managers;
namespace HoloJam
{
    public class COPlayer : CorruptableObject
    {
        [SerializeField]
        private GameObject GlobeObject;
        private Player mPlayer;
        [SerializeField]
        private string inNegationSFX;
        private bool isPlaying;
        private void Start()
        {
            BaseStart();
            mPlayer = GetComponent<Player>();
        }
        private void OnDestroy()
        {
            AudioManager.Instance.Stop(inNegationSFX);
        }
        private void Update()
        {
            if (NegationFieldsOverlapped > 0 && !isPlaying)
            {
                AudioManager.Instance.Play(inNegationSFX);
                isPlaying = true;
            } else if (isPlaying && NegationFieldsOverlapped <= 0)
            {
                AudioManager.Instance.Stop(inNegationSFX);
                isPlaying = false;
            }
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
