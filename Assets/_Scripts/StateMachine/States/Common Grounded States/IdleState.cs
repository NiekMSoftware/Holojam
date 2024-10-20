namespace HoloJam.StateMachine.States
{
    public class IdleState : State
    {
        public override void Enter()
        {
            base.Enter();
            core.charAnimator.PlayAnimation("idle");
        }

        public override void Do()
        {
            if (!core.SurroundingSensor.Grounded)
                IsComplete = true;
        }

        public override void Exit()
        {
        }
    }
}