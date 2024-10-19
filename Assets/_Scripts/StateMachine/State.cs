using UnityEngine;

namespace HoloJam.StateMachine
{
    public class State : MonoBehaviour
    {
        public bool IsComplete { get; protected set; }

        protected float startTime;
        public float RunTime => Time.time - startTime;

        public virtual void Enter() {  }
        public virtual void Exit() {  }
        public virtual void Do() {  }
        public virtual void FixedDo() {  }
    }
}
