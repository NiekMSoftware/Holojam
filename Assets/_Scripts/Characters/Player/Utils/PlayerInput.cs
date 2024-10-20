using HoloJam.Dialogue;
using HoloJam.Managers;
using UnityEngine;

namespace HoloJam.Characters.Player.Utils
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputActions;
        public PlayerInputActions.PlayerActions PlayerActions;
        public PlayerInputActions.UIActions UIActions;

        #region Main Methods

        private void Awake()
        {
            InputActions = new PlayerInputActions();
            PlayerActions = InputActions.Player;
            UIActions = InputActions.UI;
        }

        private void OnEnable()
        {
            PlayerActions.Enable();
        }

        private void OnDisable()
        {
            PlayerActions.Disable();
            UIActions.Disable();
        }

        #endregion

        public void SwitchToUIControls()
        {
            PlayerActions.Disable();
            UIActions.Enable();
        }

        public void SwitchToPlayerControls()
        {
            UIActions.Disable();
            PlayerActions.Enable();
        }

        #region Input Methods

        public float GetMovementInput()
        {
            return PlayerActions.Move.ReadValue<float>();
        }

        public float GetJumpValue()
        {
            return PlayerActions.Jump.ReadValue<float>();
        }

        public float GetInteractValue()
        {
            return PlayerActions.Interact.ReadValue<float>();
        }

        #endregion
    }
}
