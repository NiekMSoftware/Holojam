using HoloJam.Player.Utils;
using HoloJam.Player.Data;
using UnityEngine;

namespace HoloJam.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; private set; }
        public PlayerInput Input { get; private set; }

        [field: Header("Data References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }

        [Header("Ground Check")]
        [SerializeField] private Collider2D GroundCollider;
        [SerializeField] private LayerMask GroundLayer;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Input = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            Jump();
        }

        private void FixedUpdate()
        {
            CheckGround();
            Move();
            ApplyFriction();
        }

        #region Movement Methods

        private void Move()
        {
            if (!(Mathf.Abs(Input.GetMovementInput().x) > 0)) return;

            // increment velocity by acceleration and clamp within range
            float increment = Input.GetMovementInput().x * Data.Acceleration;
            float newSpeed = Mathf.Clamp(Rigidbody.linearVelocityX + increment, -Data.MaxHorizontalSpeed, Data.MaxHorizontalSpeed);
            Rigidbody.linearVelocity = new Vector2(newSpeed, Rigidbody.linearVelocityY);

            // flip object based on direction
            float direction = Mathf.Sign(Input.GetMovementInput().x);
            transform.localScale = new Vector3(direction, 1, 1);
        }

        private void Jump()
        {
            if (Input.GetJumpValue() == 0) return;

            // if the palyer is grounded, jump
            if (Data.Grounded)
                Rigidbody.linearVelocityY = Data.JumpingForce;
        }

        #endregion

        #region Movement Utils

        private void CheckGround()
        {
            Data.Grounded = Physics2D.OverlapAreaAll(GroundCollider.bounds.min, GroundCollider.bounds.max, GroundLayer).Length > 0;
        }

        private void ApplyFriction()
        {
            if (Data.Grounded && Input.GetMovementInput().x == 0 && Rigidbody.linearVelocityY <= 0)
                Rigidbody.linearVelocity *= Data.GroundDecay;
        }

        #endregion
    }
}