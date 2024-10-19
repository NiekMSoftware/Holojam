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
            if (!core.GroundSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
        }
    }
}