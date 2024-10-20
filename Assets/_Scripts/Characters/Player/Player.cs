using UnityEngine;
using HoloJam.Characters.Player.Utils;
using HoloJam.StateMachine;
using HoloJam.StateMachine.States;

namespace HoloJam.Characters.Player
{
    /// <summary>
    /// Player inherits from Core, taking all the core functionality from it.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class Player : Core
    {
        [Header("Behaviors")]
        public IdleState idleState;
        public RunState runState;
        public AirState airState;

        [field: Space()] public PlayerInput Input { get; private set; }

        // temporary place for the jump timing
        [SerializeField] private float _jumpTimeCounter;
        [SerializeField] private float jumpTime = 0.35f; // max jump time
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float extraForce = 5f;
        [Space(), SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float _coyoteTimeCounter;

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

            // Update timers if grounded
            UpdateTimers();
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
            // Allow jumping even if coyoteTimeCounter > 0 (player left ground recently)
            if (Input.GetJumpValue() > 0 && _coyoteTimeCounter > 0)
            {
                Body.linearVelocityY = jumpForce;
                _coyoteTimeCounter = 0;  // Reset coyote time after jumping
            }

            // continue to apply jump force while button is held
            if (Input.GetJumpValue() > 0 && _jumpTimeCounter > 0)
            {
                Body.linearVelocityY = Mathf.Lerp(extraForce, jumpForce, _jumpTimeCounter / jumpTime);
                _jumpTimeCounter -= Time.deltaTime;
            }

            // if the button is released early, stop the jump force
            if (Input.GetJumpValue() == 0 && _jumpTimeCounter > 0f && !Data.AirborneData.Grounded)
            {
                _jumpTimeCounter = 0;
            }
        }

        private void ApplyFriction()
        {
            if (Data.AirborneData.Grounded && Input.GetMovementInput().x == 0 && Body.linearVelocityY <= 0)
                Body.linearVelocity *= Data.GroundedData.GroundDecay;
        }

        private void UpdateTimers()
        {
            if (Data.AirborneData.Grounded)
            {
                _coyoteTimeCounter = coyoteTime;
                _jumpTimeCounter = jumpTime;
            }
            else
            {
                _coyoteTimeCounter -= Time.deltaTime;
            }
        }
        #endregion
    }
}