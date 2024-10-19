using HoloJam.Characters.Data;
using HoloJam.StateMachine;
using UnityEngine;

namespace HoloJam.Characters
{
    /// <summary>
    /// All entities inherit from Core.
    /// </summary>
    public abstract class Core : MonoBehaviour
    {
        [Header("Inherited Blackboard variabels")]
        // blackboard variables
        public Rigidbody2D Body;
        // TOOD: ANIMATOR IMPLEMENTATION
        public GroundSensor GroundSensor;
        public Machine Machine;

        [field: Header("Data References")]
        [field: SerializeField] protected CharacterSO Data { get; private set; }

        public void SetupInstances()
        {
            Machine = new Machine();

            // Gather all States and set their core.
            State[] allChildStates = GetComponentsInChildren<State>();
            foreach (State state in allChildStates)
                state.SetCore(this);
        }
    }
}