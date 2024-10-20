using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class AirState : State
    {
        public override void Enter()
        {
            // play anim
            float velY = Rigidbody.linearVelocityY;
            core.charAnimator.PlayAnimation(velY > 0 ? "jump" : "fall");
        }

        public override void Do()
        {
            // seek the animator to the frame based on the y velocity
            float velY = Rigidbody.linearVelocityY;
            core.charAnimator.PlayAnimation(velY > 0 ? "jump" : "fall");
            if (core.SurroundingSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
            
        }
    }
}
