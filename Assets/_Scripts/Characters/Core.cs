using HoloJam.Characters.Data;
using HoloJam.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace HoloJam.Characters
{
    /// <summary>
    /// All entities inherit from Core.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(SurroundingSensors))]
    public abstract class Core : MonoBehaviour
    {
        [Header("Inherited Blackboard variabels")]
        // blackboard variables
        public Rigidbody2D Body;
        // TOOD: ANIMATOR IMPLEMENTATION
        public SurroundingSensors SurroundingSensor;
        public Machine Machine;
        public State State => Machine.CurrentState;

        [field: Header("Data References")]
        [field: SerializeField] public CharacterSO Data { get; private set; }

        public void Set(State newState, bool forceReset = false)
        {
            Machine.Set(newState, forceReset);
        }

        public void SetupInstances()
        {
            Machine = new Machine();

            // Gather all States and set their core.
            State[] allChildStates = GetComponentsInChildren<State>();
            foreach (State state in allChildStates)
                state.SetCore(this);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Application.isPlaying && State != null)
            {
                List<State> states = Machine.GetActiveStateBranch();
                UnityEditor.Handles.Label(transform.position, "Active States: " + string.Join(" > ", states));
            }
#endif
        }
    }
}