using HoloJam.Characters;
using UnityEngine;

namespace HoloJam.StateMachine
{
    /// <summary>
    /// Base State class that all the other possible states inherit from.
    /// </summary>
    public class State : MonoBehaviour
    {
        public bool IsComplete { get; protected set; }

        protected float startTime;
        public float RunTime => Time.time - startTime;

        // blackboard variables
        protected Core core;
        protected Rigidbody2D Rigidbody => core.Body;

        public Machine Machine;
        public State Parent;
        public State state => Machine.CurrentState;

        protected void Set(State newState, bool forceReset = false)
        {
            Machine.Set(newState, forceReset);
        }

        public void SetCore(Core core)
        {
            this.core = core;
        }

        public void FixedDoBranch()
        {
            FixedDo();
            state?.FixedDoBranch();
        }

        public void DoBranch()
        {
            Do();
            state?.DoBranch();
        }

        public void Initialise()
        {
            IsComplete = false;
            startTime = Time.time;
        }

        public virtual void Enter() {  }
        public virtual void Exit() {  }
        public virtual void Do() {  }
        public virtual void FixedDo() {  }
    }
}
