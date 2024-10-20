using UnityEngine;
namespace HoloJam.StateMachine.States
{
    public class RunState : State
    {
        public override void Enter()
        {
            base.Enter();
            core.charAnimator.PlayAnimation("run");
        }

        public override void Do()
        {
            float velX = Rigidbody.linearVelocityX;
            if (velX != 0)
            {
                core.SetFacingLeft(velX < 0);
            }
            core.charAnimator.PlayAnimation(Mathf.Abs(velX) > 1 ? "run" : "idle");
            core.charAnimator.SetSpeed(Mathf.Abs(velX)/ core.Data.GroundedData.MaxHorizontalSpeed);
            // set anim speed with the helper's map function

            if (!core.SurroundingSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
            core.charAnimator.SetSpeed(1);
        }
    }
}
