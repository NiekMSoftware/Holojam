using HoloJam.StateMachine;
using UnityEngine;

namespace HoloJam.Characters
{
    public abstract class Core : MonoBehaviour
    {
        public Rigidbody2D Body;
        // Animator
        public GroundSensor GroundSensor;
        public Machine Machine;

        public void SetupInstances()
        {
            Machine = new Machine();

            State[] allChildStates = GetComponentsInChildren<State>();
            foreach (State state in allChildStates)
                state.SetCore(this);
        }
    }
}
