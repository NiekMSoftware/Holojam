using HoloJam.StateMachine.States;
using UnityEngine;

namespace HoloJam.Characters.NPC
{
    public class Tako : Core
    {
        public Patrol Patrol;

        public Collect Collect;

        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SetupInstances();
            Set(Patrol);
        }

        private void Update()
        {
            State.DoBranch();
            if (performingAction) return;
            if (State.IsComplete)
            {
                if (State == Collect)
                    Set(Patrol);
            }

            if (State == Patrol)
            {
                Collect.CheckForTarget();

                if (Collect.Target != null)
                    Set(Collect);
            }

            //State.DoBranch();
        }

        private void FixedUpdate()
        {
            State.FixedDoBranch();
        }
    }
}
