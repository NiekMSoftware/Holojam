using HoloJam.Dialogue;
using HoloJam.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HoloJam.Characters.Player.Utils
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance;
        public PlayerInputActions InputActions;
        public PlayerInputActions.PlayerActions PlayerActions;
        public PlayerInputActions.UIActions UIActions;

        #region Main Methods
        public static PlayerInputActions.UIActions GetUIInput()
        {
            return Instance.UIActions;
        }

        private void Awake()
        {
            InputActions = new PlayerInputActions();
            PlayerActions = InputActions.Player;
            UIActions = InputActions.UI;
            PlayerInput.Instance = this;
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
            Debug.Log("Enabled ui");

            UIActions.Submit.performed += OnSubmit;
        }

        public void EnableUIControls()
        {
            UIActions.Enable();
            Debug.Log("Enabled ui");
            PlayerActions.Jump.Disable();

            UIActions.Submit.performed += OnSubmit;
        }

        public void DisableUIControls()
        {
            UIActions.Disable();
            PlayerActions.Jump.Enable();

            UIActions.Submit.performed -= OnSubmit;
        }

        public void SwitchToPlayerControls()
        {
            UIActions.Submit.performed -= OnSubmit;
            UIActions.Disable();
            PlayerActions.Enable();
        }

        public void OnSubmit(InputAction.CallbackContext ctx)
        {
            if (UIManager.Instance == null) return;
            if (!UIManager.Instance.IsTypingComplete() && !PauseManager.IsPaused)
            {
                // If the dialogue is still typing, finish it
                if (DialogueManager.Instance.CurrentNode != null)
                    UIManager.Instance.FinishTyping(DialogueManager.Instance.CurrentNode.dialogueText);

                return;
            }

            DialogueManager.Instance.ContinueDialogue();
        }

        #region Input Methods

        public float GetMovementInput()
        {
            return PlayerActions.Move.ReadValue<float>();
        }
        public float GetUpDownInput()
        {
            return PlayerActions.UpDown.ReadValue<float>();
        }
        public bool UpdownThisFrame()
        {
            return PlayerActions.UpDown.WasPressedThisFrame();
        }
        public float GetJumpValue()
        {
            return PlayerActions.Jump.ReadValue<float>();
        }
        public bool GetJumpPressed()
        {
            return PlayerActions.Jump.ReadValue<float>() > 0 && PlayerActions.Jump.WasPressedThisFrame();
        }

        public bool GetInteractionPressed()
        {
            return PlayerActions.Interact.ReadValue<float>() > 0 && PlayerActions.Interact.WasPressedThisFrame();
        }
        public float GetInteractValue()
        {
            return PlayerActions.Interact.ReadValue<float>();
        }
        public bool GetCorruptionPressed()
        {
            return PlayerActions.Corruption.WasPressedThisFrame();
        }
        public bool GetPausePressed()
        {
            return PlayerActions.Pause.WasPressedThisFrame();
        }

        #endregion
    }
}
