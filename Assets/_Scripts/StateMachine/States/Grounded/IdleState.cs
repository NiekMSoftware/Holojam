using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class IdleState : State
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Do()
        {
            if (!Input.Data.Grounded || Input.Input.GetMovementInput().x != 0)
                IsComplete = true;
        }

        public override void Exit()
        {
        }
    }
}