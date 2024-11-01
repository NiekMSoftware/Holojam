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
        public string LastAnimationBase { get { return lastAnimationBase; } }
        private string lastAnimationBase = "idle";
        private string prefix = "";
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
            if (LastAnimationBase != "die")
            {
                PlayAnimation(lastAnimationBase, newFacingLeft);
            }
        }

         public void PlayAnimation(string animation)
        {
            PlayAnimation(animation, characterRef.lastFacingLeft);
        }

        public void SetSpeed(float speed)
        {
            Animator.speed = speed;
        }
        public void SetPrefix(string newPrefix)
        {
            prefix = newPrefix;
        }
        public void PlayAnimation(string animation, bool isFacingLeft = false)
        {
            lastAnimationBase = animation;
            string targetAnimation = animation;
            string appendedSuffix = targetAnimation + (isFacingLeft ? left_suffix : right_suffix);
            int animHash = Animator.StringToHash(appendedSuffix);
            bool hasState = Animator.HasState(0, animHash);
            if (hasState)
            {
                targetAnimation = appendedSuffix;
            }
            
            string appendedPrefix = prefix + targetAnimation;
            animHash = Animator.StringToHash(appendedPrefix);
            hasState = Animator.HasState(0, animHash);
            if (hasState)
            {
                targetAnimation = appendedPrefix;
            }
            Animator.Play(targetAnimation);
            Animator.Update(0); // force update;
        }
    }
}
