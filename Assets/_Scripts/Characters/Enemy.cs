using HoloJam.StateMachine.States;
using UnityEngine;

namespace HoloJam.Characters.Enemies
{
    public class Enemy : Core
    {
        public enum AdditionalStates
        {
            None,
            Frozen,
            AntiGravity
        }

        [field: Header("Additional Enemy Properties")]
        [field: SerializeField] public AdditionalStates AdditionalEnemyState { get; private set; }

        [Space]
        [SerializeField] private IdleState Idle;

        // internal flags
        private bool _isFrozen = false;

        protected virtual void Awake()
        {
            Body = GetComponent <Rigidbody2D>();
            CharacterAnimator = GetComponent<CharacterAnimation>();

            SetupInstances();
            Machine.Set(Idle);
        }

        protected virtual void Update()
        {
            HandleState();
        }

        /// <summary>
        /// Handles additional states (e.g. anti-gravity, frozen)
        /// </summary>
        private void HandleState()
        {
            switch (AdditionalEnemyState)
            {
                case AdditionalStates.None:
                    if (_isFrozen)
                    {
                        UnfreezeEnemy();
                        _isFrozen = false;
                    }
                    DisableAntiGravity();
                    break;

                case AdditionalStates.Frozen:
                    if (!_isFrozen)
                    {
                        FreezeEnemy();
                        _isFrozen = true;
                    }
                    break;

                case AdditionalStates.AntiGravity:
                    if (_isFrozen)
                    {
                        UnfreezeEnemy();
                    }
                    EnableAntiGravity();
                    break;
            }
        }

        private void FreezeEnemy()
        {
            // Stop all movement
            Body.linearVelocity = Vector2.zero;
            Body.bodyType = RigidbodyType2D.Kinematic;

            // Pause animations
            CharacterAnimator.Animator.enabled = false;
        }

        private void UnfreezeEnemy()
        {
            Body.bodyType = RigidbodyType2D.Dynamic;
            CharacterAnimator.Animator.enabled = true;
        }

        private void EnableAntiGravity()
        {
            Body.gravityScale = -1f;
        }

        private void DisableAntiGravity()
        {
            Body.gravityScale = 1f;
        }
    }
}
