using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class AirState : State
    {
        public override void Enter()
        {
            // play anim
        }

        public override void Do()
        {
            // seek the animator to the frame based on the y velocity
            
            if (core.GroundSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
            
        }
    }
}
