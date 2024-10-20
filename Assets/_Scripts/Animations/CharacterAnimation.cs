using UnityEngine;
using HoloJam.Characters;

namespace HoloJam
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField]
        private Core playerRef;
        [SerializeField]
        private Animator mAnimator;
        [SerializeField]
        private string left_suffix;
        [SerializeField]
        private string right_suffix;

        private string lastAnimationBase = "idle";
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            playerRef.DirectionChangedEvent += OnDirectionChanged;
        }
        private void OnDestroy()
        {
            playerRef.DirectionChangedEvent -= OnDirectionChanged;
        }
        public void OnDirectionChanged(bool newFacingLeft)
        {
            PlayAnimation(lastAnimationBase, newFacingLeft);
        }
        public void PlayAnimation(string animation)
        {
            PlayAnimation(animation, playerRef.lastFacingLeft);
        }
        public void SetSpeed(float speed)
        {
            mAnimator.speed = speed;
        }
        public void PlayAnimation(string animation, bool isFacingLeft = false)
        {
            lastAnimationBase = animation;
            string appendedSuffix = animation + (isFacingLeft ? left_suffix : right_suffix);
            int animHash = Animator.StringToHash(appendedSuffix);
            bool hasState = mAnimator.HasState(0, animHash);
            if (hasState)
            {
                mAnimator.Play(appendedSuffix);
            } else
            {
                mAnimator.Play(lastAnimationBase);
            }
        }
    }
}
