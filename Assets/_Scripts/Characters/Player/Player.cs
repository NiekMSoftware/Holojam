using HoloJam.Player.Utils;
using UnityEngine;

namespace HoloJam.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; private set; }
        public PlayerInput Input { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Input = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            
        }

        private void Move()
        {
            // increment velocity by acceleration
        }
    }

}