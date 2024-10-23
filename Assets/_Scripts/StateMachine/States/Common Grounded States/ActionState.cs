using UnityEngine;
namespace HoloJam.StateMachine.States
{
    public class ActionState : State
    {
        [SerializeField]
        private Attackable attackable;
        public override void Enter()
        {
            base.Enter();
        }

        public override void Do()
        {
            if (CharAnimator.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
            {
                core.performingAction = false;
                IsComplete = true;
                if (CharAnimator.LastAnimationBase == "die" && attackable != null)
                {
                    attackable.OnDeathAnimationEnd();
                }
            }
        }

        public override void Exit()
        {
        }

    }
}
