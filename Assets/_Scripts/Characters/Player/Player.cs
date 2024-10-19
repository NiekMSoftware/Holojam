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

        [field: Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Input = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            // increment velocity by acceleration and clamp within range
            float increment = Input.GetMovementInput().x * Data.Acceleration;
            float newSpeed = Mathf.Clamp(Rigidbody.linearVelocityX + increment, -Data.MaxHorizontalSpeed, Data.MaxHorizontalSpeed);
            Rigidbody.linearVelocity = new Vector2(newSpeed, Rigidbody.linearVelocityY);
        }
    }

}