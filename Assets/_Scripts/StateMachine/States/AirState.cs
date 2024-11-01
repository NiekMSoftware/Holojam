using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class AirState : State
    {
        COPlayer player;
        public override void Enter()
        {
            // play anim
            float velY = Rigidbody.linearVelocityY;
            CharAnimator.PlayAnimation(velY > 0 ? "jump" : "fall");
            player = core.GetComponent<COPlayer>();
        }

        public override void Do()
        {
            // seek the animator to the frame based on the y velocity
            float velY = Rigidbody.linearVelocityY;
            float velX = Rigidbody.linearVelocityX;
            if (player != null &&
                player.InvertedGravity)
            {
                CharAnimator.PlayAnimation(velY > 0 ? "fall" : "jump");
            } else
            {
                CharAnimator.PlayAnimation(velY > 0 ? "jump" : "fall");
            }
            if (core.SurroundingSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
            
        }
    }
}
