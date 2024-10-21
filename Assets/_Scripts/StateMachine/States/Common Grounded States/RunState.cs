using UnityEngine;
namespace HoloJam.StateMachine.States
{
    public class RunState : State
    {
        public override void Enter()
        {
            base.Enter();
            CharAnimator.PlayAnimation("run");
        }

        public override void Do()
        {
            float velX = Rigidbody.linearVelocityX;
            if (velX != 0)
            {
                core.SetFacingLeft(velX < 0);
            }
            CharAnimator.PlayAnimation(Mathf.Abs(velX) > 1 ? "run" : "idle");
            CharAnimator.SetSpeed(Mathf.Abs(velX)/ core.Data.GroundedData.MaxHorizontalSpeed);

            if (!core.SurroundingSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
            CharAnimator.SetSpeed(1);
        }
    }
}
