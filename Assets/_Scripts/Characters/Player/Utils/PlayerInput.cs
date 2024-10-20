using UnityEngine;

namespace HoloJam.Characters.Player.Utils
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputActions;
        public PlayerInputActions.PlayerActions PlayerActions;

        #region Main Methods

        private void Awake()
        {
            InputActions = new PlayerInputActions();
            PlayerActions = InputActions.Player;
        }

        private void OnEnable()
        {
            PlayerActions.Enable();
        }

        private void OnDisable()
        {
            PlayerActions.Disable();
        }

        #endregion

        #region Input Methods

        public float GetMovementInput()
        {
            return PlayerActions.Move.ReadValue<float>();
        }

        public float GetJumpValue()
        {
            return PlayerActions.Jump.ReadValue<float>();
        }

        #endregion
    }
}
