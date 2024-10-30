using System.Collections.Generic;
using UnityEngine;

namespace HoloJam.StateMachine.States
{
    public class Collect : State
    {
        public List<Transform> Targets;

        // Check for any souls in the vicinity
        public Transform Target;

        // If you find one, run over to it
        public Navigate Navigate;

        // Idle for one second afterward, then complete
        public IdleState Idle;

        public float CollectRadius;
        public float Vision = 1f;

        public override void Enter()
        {
            Navigate.destination = Target.position;
            Set(Navigate, true);
        }

        public override void Do()
        {
            if (state == Navigate)
            {
                if (CloseEnough(Target.position))
                {
                    Set(Idle, true);
                    Rigidbody.linearVelocity = new Vector2(0, Rigidbody.linearVelocityY);
                    Target.gameObject.SetActive(false);
                }
                else if (!InVision(Target.position))
                {
                    Set(Idle, true);
                    Rigidbody.linearVelocity = new Vector2(0, Rigidbody.linearVelocityY);
                }
                else
                {
                    Navigate.destination = Target.position;
                    Set(Navigate, true);
                }
            }
            else
            {
                if (state.RunTime > 2)
                {
                    IsComplete = true;
                }
            }

            if (Target == null)
            {
                IsComplete = true;
                return;
            }
        }

        public override void Exit()
        {
        }

        public bool CloseEnough(Vector2 targetPos)
        {
            return Vector2.Distance(core.transform.position, targetPos) < CollectRadius;
        }

        public bool InVision(Vector2 targetPos)
        {
            return Vector2.Distance(core.transform.position, targetPos) < Vision;
        }

        public void CheckForTarget()
        {
            if (Targets == null) return;
            foreach (Transform t in Targets)
            {
                if (t == null) continue;
                if (InVision(t.position) && t.gameObject.activeSelf)
                {
                    Target = t;
                    return;
                }
            }

            Target = null;
        }
    }
}
