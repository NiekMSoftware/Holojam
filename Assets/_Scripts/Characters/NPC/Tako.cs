using UnityEngine;

namespace HoloJam.Characters.NPC
{
    public class Tako : Core
    {
        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SetupInstances();
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
