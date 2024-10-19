using HoloJam.Player.Utils;
using HoloJam.Player.Data;
using UnityEngine;
using HoloJam.StateMachine;
using HoloJam.StateMachine.States;
using HoloJam.Characters;

namespace HoloJam.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class Player : Core
    {
        [Header("Behaviors")]
        public IdleState idleState;
        public RunState runState;
        public AirState airState;

        [field: Space()] public PlayerInput Input { get; private set; }

        [field: Header("Data References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }

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
            Jump();

            SelectState();
            Machine.CurrentState.Do();
        }

        private void FixedUpdate()
        {
            Move();
            ApplyFriction();
        }

        #region State Machine
        private void SelectState()
        {
            // grounded states
            if (GroundSensor.Grounded)
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
        #endregion

        #region Movement Methods
        private void Move()
        {
            if (!(Mathf.Abs(Input.GetMovementInput().x) > 0)) return;

            // increment velocity by acceleration and clamp within range
            float increment = Input.GetMovementInput().x * Data.Acceleration;
            float newSpeed = Mathf.Clamp(Body.linearVelocityX + increment, -Data.MaxHorizontalSpeed, Data.MaxHorizontalSpeed);
            Body.linearVelocity = new Vector2(newSpeed, Body.linearVelocityY);

            // flip object based on direction
            float direction = Mathf.Sign(Input.GetMovementInput().x);
            transform.localScale = new Vector3(direction, 1, 1);
        }

        private void Jump()
        {
            if (Input.GetJumpValue() == 0) return;

            // if the palyer is grounded, jump
            if (GroundSensor.Grounded)
                Body.linearVelocityY = Data.JumpingForce;
        }
        #endregion

        private void ApplyFriction()
        {
            if (GroundSensor.Grounded && Input.GetMovementInput().x == 0 && Body.linearVelocityY <= 0)
                Body.linearVelocity *= Data.GroundDecay;
        }
    }
}