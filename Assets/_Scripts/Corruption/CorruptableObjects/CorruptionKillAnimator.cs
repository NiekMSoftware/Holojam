using UnityEngine;

namespace HoloJam
{
    public class CorruptionKillAnimator : MonoBehaviour
    {
        public bool KillNow = false;
        public bool useGravityOverride = false;
        public bool useGravity = true;
        [SerializeField]
        private string killAnim = "kill";
        private Animator mAnimator;
        private Rigidbody2D rigidbody2D;
        private bool lastGravity;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            mAnimator = GetComponent<Animator>();
            rigidbody2D = GetComponent <Rigidbody2D>();
            lastGravity = useGravity;
        }
        public void StartKill()
        {
            mAnimator.Play(killAnim);
        }
        // Update is called once per frame
        void Update()
        {
            if (useGravityOverride && lastGravity != useGravity)
            {
                rigidbody2D.gravityScale = useGravity ? 1 : 0;
                lastGravity = useGravity;
            }
            if (KillNow)
            {
                Destroy(gameObject);
            }
        }
    }
}
