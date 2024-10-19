namespace HoloJam.StateMachine.States
{
    public class RunState : State
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Do()
        {
            float velX = Rigidbody.linearVelocityX;
            // set anim speed with the helper's map function

            if (!core.GroundSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
            
        }
    }
}
