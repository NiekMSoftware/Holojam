using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class Navigate : State
    {
        public Vector2 destination;
        public float threshold = 0.1f;
        public State Animation;
        public bool allowYMovement;
        public override void Enter()
        {
            Set(Animation, true);
        }

        public override void Do()
        {
            float dist = Vector2.Distance(core.transform.position, destination);
            if (dist < threshold)
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
            float newYSpeed = core.Body.linearVelocityY;
            if (allowYMovement)
            {
                increment = direction.y * core.Data.GroundedData.Acceleration;
                newYSpeed = Mathf.Clamp(Rigidbody.linearVelocityY + increment, -core.Data.GroundedData.MaxHorizontalSpeed, core.Data.GroundedData.MaxHorizontalSpeed);
            }
            if (core.GetComponent<CorruptableObject>().Frozen)
            {
                Rigidbody.linearVelocity = Vector2.zero;
            } else
            {
                Rigidbody.linearVelocity = new Vector2(newSpeed, newYSpeed);
            }
        }
    }
}
