using System.Collections.Generic;

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
                CurrentState.Initialise(this);
                CurrentState.Enter();
            }
        }

        public List<State> GetActiveStateBranch(List<State> list = null)
        {
            if (list == null)
            {
                list = new List<State>();
            }

            if (CurrentState == null)
            {
                return list;
            }
            else
            {
                list.Add(CurrentState);
                return CurrentState.Machine.GetActiveStateBranch(list);
            }
        }
    }
}
