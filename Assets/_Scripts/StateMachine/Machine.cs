namespace HoloJam.StateMachine
{
    /// <summary>
    /// State Machine class that sets the new State.
    /// </summary>
    public class Machine
    {
        public State CurrentState;

        public void Set(State newState, bool forceReset = false)
        {
            if (CurrentState != newState || forceReset) {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentState.Initialise();
                CurrentState.Enter();
            }
        }
    }
}
