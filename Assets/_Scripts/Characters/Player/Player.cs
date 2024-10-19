using UnityEngine;
using HoloJam.Characters.Player.Utils;
using HoloJam.StateMachine;
using HoloJam.StateMachine.States;

namespace HoloJam.Characters.Player
{
    /// <summary>
    /// Player inherits from Core, taking all the core functionality from it.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class Player : Core
    {
        [Header("Behaviors")]
        public IdleState idleState;
        public RunState runState;
        public AirState airState;

        [field: Space()] public PlayerInput Input { get; private set; }

        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Input = GetComponent<PlayerInput>();
            GroundSensor = GetComponent<GroundSensor>();
        }

        private void Start()
        {
            SetupInstances();
            Machine.Set(idleState);
        }

        private void Update()
        {
            // Handle state machine
            SelectState();
            Machine.CurrentState.Do();
            
            Jump();
        }

        private void FixedUpdate()
        {
            Move();
            ApplyFriction();
        }

        private void SelectState()
        {
            Data.AirborneData.Grounded = GroundSensor.Grounded;

            // grounded states
            if (Data.AirborneData.Grounded)
            {
                if (Input.GetMovementInput().x == 0)
                {
                    Machine.Set(idleState);
                }
                else
                    Machine.Set(runState);
            }
            else
                Machine.Set(airState);
        }

        #region Movement Methods
        private void Move()
        {
            if (!(Mathf.Abs(Input.GetMovementInput().x) > 0)) return;

            // increment velocity by acceleration and clamp within range
            float increment = Input.GetMovementInput().x * Data.GroundedData.Acceleration;
            float newSpeed = Mathf.Clamp(Body.linearVelocityX + increment, -Data.GroundedData.MaxHorizontalSpeed, Data.GroundedData.MaxHorizontalSpeed);
            Body.linearVelocity = new Vector2(newSpeed, Body.linearVelocityY);

            // flip object based on direction
            float direction = Mathf.Sign(Input.GetMovementInput().x);
            transform.localScale = new Vector3(direction, 1, 1);
        }

        private void Jump()
        {
            if (Input.GetJumpValue() == 0) return;

            // if the palyer is grounded, jump
            if (Data.AirborneData.Grounded)
                Body.linearVelocityY = Data.AirborneData.JumpingForce;
        }

        private void ApplyFriction()
        {
            if (Data.AirborneData.Grounded && Input.GetMovementInput().x == 0 && Body.linearVelocityY <= 0)
                Body.linearVelocity *= Data.GroundedData.GroundDecay;
        }
        #endregion
    }
}