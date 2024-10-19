using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class Navigate : State
    {
        public Vector2 destination;
        public float threshold = 0.1f;
        public State Animation;

        public override void Enter()
        {
            Set(Animation, true);
        }

        public override void Do()
        {
            if (Vector2.Distance(core.transform.position, destination) < threshold)
            {
                IsComplete = true;
            }
            core.transform.localScale = new Vector3(Mathf.Sign(Rigidbody.linearVelocityX), 1, 1);
        }

        public override void FixedDo()
        {
            Vector2 direction = (destination - (Vector2)core.transform.position).normalized;

            float increment = direction.x * core.Data.GroundedData.Acceleration;
            float newSpeed = Mathf.Clamp(Rigidbody.linearVelocityX + increment, -core.Data.GroundedData.MaxHorizontalSpeed, core.Data.GroundedData.MaxHorizontalSpeed);
            Rigidbody.linearVelocity = new Vector2(newSpeed, core.Body.linearVelocityY);
        }
    }
}
