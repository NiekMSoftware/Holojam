using UnityEngine;

namespace HoloJam.StateMachine
{
    public class State : MonoBehaviour
    {
        public bool IsComplete { get; protected set; }

        protected float startTime;
        public float RunTime => Time.time - startTime;

        // blackboard variables
        protected Rigidbody2D Rigidbody;
        protected Player.Player Input;

        public virtual void Enter() {  }
        public virtual void Exit() {  }
        public virtual void Do() {  }
        public virtual void FixedDo() {  }

        public void Setup(Rigidbody2D body, Player.Player player)
        {
            Rigidbody = body;
            Input = player;
        }

        public void Initialise()
        {
            IsComplete = false;
            startTime = Time.time;
        }
    }
}
