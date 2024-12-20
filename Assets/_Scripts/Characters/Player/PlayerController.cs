using HoloJam.Characters.Data;
using HoloJam.Characters.Player.Utils;
using UnityEngine;
using UnityEngine.Windows;

namespace HoloJam.Characters.Player
{
    /// <summary>
    /// Handles everything to control the player, mainly moving and jumping around.
    /// Requires a Rigidbody 2D attached to the Controller to function properly.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {        
        private Rigidbody2D _body;

        // timers
        private float _jumpTimeCounter;
        private float _coyoteTimeCounter;

        // parent component
        private Player player;
        private void Start()
        {
            player = GetComponent<Player>();
        }

        public void Initialize(Rigidbody2D body)
        {
            _body = body;
        }

        public void Move(float movementInput,  float acceleration, float maxSpeed)
        {
            // return if no input is given
            if (!(Mathf.Abs(movementInput) > 0)) return;

            // increment velocity by acceleration
            float increment = movementInput * acceleration;
            float newSpeed = Mathf.Clamp(_body.linearVelocityX + increment, -maxSpeed, maxSpeed);
            _body.linearVelocityX = newSpeed;

            // flip object based on direction
            float direction = Mathf.Sign(movementInput);
            if (!player.performingAction)
            {
                player.SetFacingLeft(direction < 0);
                transform.localScale = new Vector3(direction, transform.localScale.y, 1);
            }
        }
        public void UpDownMove(float updowninput, float acceleration, float maxSpeed)
        {
            // return if no input is given
            if (!(Mathf.Abs(updowninput) > 0)) return;

            // increment velocity by acceleration
            float increment = updowninput * acceleration;
            float newSpeed = Mathf.Clamp(_body.linearVelocityY + increment, -maxSpeed, maxSpeed);
            _body.linearVelocityY = newSpeed;
        }

        public void HandleJump(PlayerInput input, AirborneData airborneData, bool normalGravity)
        {
            float jumpMod = (normalGravity ? 1 : -1);
            // Start the initial jump
            if (input.GetJumpPressed() && _coyoteTimeCounter > 0)
            {
                _body.linearVelocityY = airborneData.JumpForce * jumpMod;
                _coyoteTimeCounter = 0;

                _jumpTimeCounter = airborneData.JumpTime;
            }

            // check if the ceiling was hit
            if (!player.SurroundingSensor.HitCeiling)
                Jump(input.GetJumpValue(), airborneData.JumpForce * jumpMod, airborneData.AddedJumpForce * jumpMod, airborneData.JumpTime);
            else
                _jumpTimeCounter = 0;

            // stop jumping if released earlier
            if (input.GetJumpValue() == 0 && _jumpTimeCounter > 0 && !airborneData.Grounded)
            {
                _jumpTimeCounter = 0;
            }

            // apply a downwards force to fall faster
            if (_jumpTimeCounter <= 0)
            {
                _body.linearVelocityY += airborneData.GravityMultiplier * Physics2D.gravity.y * Time.deltaTime * jumpMod;

                // Clamp the falling velocity to avoid falling too fast
                _body.linearVelocityY = Mathf.Clamp(_body.linearVelocityY, -airborneData.MaxFallingSpeed, Mathf.Infinity);
            }
        }

        private void Jump(float jumpValue, float jumpForce, float addedJumpForce, float jumpTime)
        {
            // When jump is being held, jump higher
            if (jumpValue > 0 && _jumpTimeCounter > 0)
            {
                _body.linearVelocity = new Vector2(_body.linearVelocity.x, Mathf.Lerp(addedJumpForce, jumpForce, _jumpTimeCounter / jumpTime));
                _jumpTimeCounter -= Time.deltaTime;
            }
        }

        public void ApplyFriction(bool isGrounded, float input, float groundDecay, float airDecay)
        {
            if (isGrounded && input == 0 && Mathf.Abs(_body.linearVelocityX) > 0.05f)
            { 
                _body.linearVelocityX *= groundDecay;

                // Stop the player if the velocity is small after decay
                if (CheckVelocity(0.1f))
                    _body.linearVelocityX = 0;
            } else if (!isGrounded && input == 0 && Mathf.Abs(_body.linearVelocityX) > 0.05f)
            {
                _body.linearVelocityX *= airDecay;

                // Stop the player if the velocity is small after decay
                if (CheckVelocity(0.1f))
                    _body.linearVelocityX = 0;
            }
        }

        public void UpdateTimers(AirborneData airborneData)
        {
            if (airborneData.Grounded)
            {
                _coyoteTimeCounter = airborneData.CoyoteTime;
                _jumpTimeCounter = airborneData.JumpTime;
            }
            else
                _coyoteTimeCounter -= Time.deltaTime;
        }

        /// <summary>
        /// Check's the absolute value of the Rigidbody's Velocity X within a certain threshold.
        /// </summary>
        /// <param name="threshold">The threshold to be met</param>
        /// <returns>Either true or false based on the absolute value.</returns>
        public bool CheckVelocity(float threshold)
        {
            return Mathf.Abs(_body.linearVelocityX) < threshold;
        }
    }
}
