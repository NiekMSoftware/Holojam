using UnityEngine;

namespace HoloJam.StateMachine
{
    /// <summary>
    /// Determines if the entity is grounded, a core component for the Core class.
    /// </summary>
    public class GroundSensor : MonoBehaviour
    {
        [Header("Ground Check")]
        [SerializeField] private Collider2D GroundCheck;
        [SerializeField] private LayerMask GroundLayer;
        public bool Grounded;

        private void FixedUpdate()
        {
            CheckGround();    
        }

        private void CheckGround()
        {
            Grounded = Physics2D.OverlapAreaAll(GroundCheck.bounds.min, GroundCheck.bounds.max, GroundLayer).Length > 0;
        }
    }
}
