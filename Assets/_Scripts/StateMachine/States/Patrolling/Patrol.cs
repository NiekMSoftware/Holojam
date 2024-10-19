using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class Patrol : State
    {
        public Navigate Navigate;
        public IdleState Idle;

        [Space]
        public Transform Anchor1;
        public Transform Anchor2;

        private void GoToNextDestination()
        {
            // switch between the two anchors
            if (Navigate.destination == (Vector2)Anchor1.position)
            {
                Navigate.destination = (Vector2)Anchor2.position;
            }
            else
                Navigate.destination = (Vector2)Anchor1.position;

            Set(Navigate, true);
        }

        public override void Enter()
        {
            GoToNextDestination();
        }

        public override void Do()
        {
            if (Machine.CurrentState == Navigate) // idle
            {
                if (Navigate.IsComplete)
                {
                    Set(Idle, true);
                    Rigidbody.linearVelocity = new Vector2(0, Rigidbody.linearVelocityY);
                }    
            }
            else
            {
                if (Machine.CurrentState.RunTime > 1) // navigate
                {
                    GoToNextDestination();
                }
            }
        }
    }
}
