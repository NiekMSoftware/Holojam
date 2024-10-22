using UnityEngine;
using HoloJam.Characters;

namespace HoloJam
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Core characterRef;
        [field: SerializeField] public Animator Animator { get; private set; }
        [SerializeField] private string left_suffix;
        [SerializeField] private string right_suffix;

        private string lastAnimationBase = "idle";

        void Start()
        {
            characterRef.DirectionChangedEvent += OnDirectionChanged;
        }

        private void OnDestroy()
        {
            characterRef.DirectionChangedEvent -= OnDirectionChanged;
        }

        public void OnDirectionChanged(bool newFacingLeft)
        {
            PlayAnimation(lastAnimationBase, newFacingLeft);
        }

        public void PlayAnimation(string animation)
        {
            PlayAnimation(animation, characterRef.lastFacingLeft);
        }

        public void SetSpeed(float speed)
        {
            Animator.speed = speed;
        }

        public void PlayAnimation(string animation, bool isFacingLeft = false)
        {
            lastAnimationBase = animation;
            string appendedSuffix = animation + (isFacingLeft ? left_suffix : right_suffix);
            int animHash = Animator.StringToHash(appendedSuffix);
            bool hasState = Animator.HasState(0, animHash);
            if (hasState)
            {
                Animator.Play(appendedSuffix);
            } else
            {
                Animator.Play(lastAnimationBase);
            }
        }
    }
}
