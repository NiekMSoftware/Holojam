using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class BirdState : State
    {
        public override void Enter()
        {
            // play anim
            //float velY = Rigidbody.linearVelocityY;
            //CharAnimator.PlayAnimation(velY > 0 ? "jump" : "fall");
        }

        public override void Do()
        {
            float velX = Rigidbody.linearVelocityX;
            float velY = Rigidbody.linearVelocityY;
            if (Mathf.Abs(velX) > Mathf.Abs(velY))
            {
                CharAnimator.PlayAnimation("bird_move");
            } else if (velY != 0)
            {
                CharAnimator.PlayAnimation(velY > 0 ? "bird_up" : "bird_down");

            } else
            {
                CharAnimator.PlayAnimation("bird_idle");
            }
        }

        public override void Exit()
        {

        }
    }
}
