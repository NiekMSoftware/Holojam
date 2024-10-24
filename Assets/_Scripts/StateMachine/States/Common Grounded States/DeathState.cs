using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class DeathState : State
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Do()
        {
            if (CharAnimator.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
                Destroy(CharAnimator.gameObject);
        }

        public override void Exit()
        {
        }
    }
}
