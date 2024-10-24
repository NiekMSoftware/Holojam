using UnityEngine;
using HoloJam.Characters.Player.Utils;
using HoloJam.StateMachine;
using HoloJam.StateMachine.States;

namespace HoloJam.Characters.Player
{
    /// <summary>
    /// Player inherits from Core, taking all the core functionality from it.
    /// Player requires the custom PlayerInput and PlayerController class to function properly.
    /// </summary>
    [RequireComponent(typeof(PlayerInput), typeof(PlayerController))]
    public class Player : Core
    {
        [Header("Behaviors")]
        public IdleState idleState;
        public RunState runState;
        public AirState airState;

        [field: Space()] public PlayerInput Input { get; private set; }
        public PlayerController Controller { get ; private set; }

        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Input = GetComponent<PlayerInput>();
            CharacterAnimator = GetComponent<CharacterAnimation>();
            SurroundingSensor = GetComponent<SurroundingSensors>();
            Controller = GetComponent<PlayerController>();
            Controller.Initialize(Body);
        }

        private void Start()
        {
            SetupInstances();
            Machine.Set(idleState);
        }

        private void Update()
        {
            TEMPActivatePower();
            // Handle state machine
            SelectState();
            Machine.CurrentState.Do();
            Controller.UpdateTimers(Data.AirborneData);
            if (performingAction) return;
            Controller.HandleJump(Input, Data.AirborneData);
        }
        private void TEMPActivatePower()
        {
            if (Input.GetCorruptionPressed()) {
                CorruptionManager.TogglePanelOpen();
            }
        }
        private void FixedUpdate()
        {
            Controller.Move(Input.GetMovementInput(), Data.GroundedData.Acceleration, Data.GroundedData.MaxHorizontalSpeed);
            Controller.ApplyFriction(Data.AirborneData.Grounded, Input.GetMovementInput(), Data.GroundedData.GroundDecay);
        }

        private void SelectState()
        {
            if (performingAction) return;
            Data.AirborneData.Grounded = SurroundingSensor.Grounded;
            if (Data.AirborneData.Grounded)
            {
                if (Input.GetMovementInput() == 0 && Controller.CheckVelocity(0.1f))
                {
                    Machine.Set(idleState);
                }
                else
                    Machine.Set(runState);
            }
            else
                Machine.Set(airState);
        }
    }
}