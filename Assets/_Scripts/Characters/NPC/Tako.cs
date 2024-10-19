using HoloJam.StateMachine.States;
using UnityEngine;

namespace HoloJam.Characters.NPC
{
    public class Tako : Core
    {
        public Patrol Patrol;

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
            if (State.IsComplete)
            {

            }

            State.DoBranch();
        }

        private void FixedUpdate()
        {
            State.FixedDoBranch();
        }
    }
}
