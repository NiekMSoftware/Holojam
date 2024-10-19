using Unity.VisualScripting;
using UnityEngine;

namespace HoloJam.StateMachine
{
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
