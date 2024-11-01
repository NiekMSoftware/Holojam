using UnityEngine;
namespace HoloJam.StateMachine.States
{
    public class RunState : State
    {
        [SerializeField]
        private string runAnim = "run";
        [SerializeField]
        private bool scaleAnimOnXSpeed = true;
        public override void Enter()
        {
            base.Enter();
            CharAnimator.PlayAnimation(runAnim);
        }

        public override void Do()
        {
            float velX = Rigidbody.linearVelocityX;
            CharAnimator.PlayAnimation(Mathf.Abs(velX) > 1 ? "run" : "idle");
            if (scaleAnimOnXSpeed)
            {
                CharAnimator.SetSpeed(Mathf.Abs(velX) / core.Data.GroundedData.MaxHorizontalSpeed);
            }

            if (!core.SurroundingSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
            CharAnimator.SetSpeed(1);
        }
    }
}
