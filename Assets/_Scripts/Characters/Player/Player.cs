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
        public BirdState birdState;

        [field: Space()] public PlayerInput Input { get; private set; }
        public PlayerController Controller { get ; private set; }
        private bool birdMode = false;
        private COPlayer corruptObj;
        private Interactor mInteractor;
        public void ToggleBird()
        {
            SetBird(!birdMode);
        }
        public void SetBird(bool bird)
        {
            birdMode = bird;
            if (bird)
            {
                mInteractor.HandsFree = false;
                Body.gravityScale = 0;
            } else
            {
                mInteractor.HandsFree = true;
                Body.gravityScale = corruptObj.InvertedGravity ? -1 : 1;
            }
        }
        private void ExitBird(Hitbox hb)
        {
            if (birdMode)
            {
                SetBird(false);
            }
        }
        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Input = GetComponent<PlayerInput>();
            CharacterAnimator = GetComponent<CharacterAnimation>();
            SurroundingSensor = GetComponent<SurroundingSensors>();
            Controller = GetComponent<PlayerController>();
            corruptObj = GetComponent<COPlayer>();
            mInteractor = GetComponent<Interactor>();
            Controller.Initialize(Body);
            GetComponent<Attackable>().hitEvent += ExitBird;
        }

        private void Start()
        {
            SetupInstances();
            Machine.Set(idleState);
        }

        private void Update()
        {
            if (PauseManager.IsPaused) return;

            TEMPActivatePower();
            //if (birdMode && Input.GetInteractValue() > 0)
            //{
            //    SetBird(false);
            //}
            // Handle state machine
            SelectState();
            Machine.CurrentState.Do();
            Controller.UpdateTimers(Data.AirborneData);
            if (performingAction) return;
            if (!birdMode)
            {
                Controller.HandleJump(Input, Data.AirborneData, !corruptObj.InvertedGravity);
            } 
        }

        private void TEMPActivatePower()
        {
            if (Input.GetCorruptionPressed() && !PauseManager.IsPaused) {
                CorruptionManager.TogglePanelOpen();
            } else if (Input.GetPausePressed() && !CorruptionManager.IsOpen)
            {
                PauseManager.TogglePause();
            }
        }
        private void FixedUpdate()
        {
            Controller.Move(Input.GetMovementInput(), Data.GroundedData.Acceleration, Data.GroundedData.MaxHorizontalSpeed);
            if (birdMode)
            {
                Controller.UpDownMove(Input.GetUpDownInput(), Data.GroundedData.Acceleration, Data.GroundedData.MaxHorizontalSpeed);
            }
            Controller.ApplyFriction(Data.AirborneData.Grounded, Input.GetMovementInput(), Data.GroundedData.GroundDecay, Data.GroundedData.AirDecay);
        }

        private void SelectState()
        {
            if (performingAction) return;
            
            Data.AirborneData.Grounded = SurroundingSensor.Grounded;
            if (birdMode)
            {
                Machine.Set(birdState);
                return;
            }
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