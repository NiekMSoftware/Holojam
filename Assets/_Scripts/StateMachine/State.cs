using HoloJam.Characters;
using UnityEngine;

namespace HoloJam.StateMachine
{
    public class State : MonoBehaviour
    {
        public bool IsComplete { get; protected set; }

        protected float startTime;
        public float RunTime => Time.time - startTime;

        // blackboard variables
        protected Core core;

        protected Rigidbody2D Rigidbody => core.Body;

        public virtual void Enter() {  }
        public virtual void Exit() {  }
        public virtual void Do() {  }
        public virtual void FixedDo() {  }

        public void SetCore(Core core)
        {
            this.core = core;
        }

        public void Initialise()
        {
            IsComplete = false;
            startTime = Time.time;
        }
    }
}
