using UnityEngine;
using HoloJam.Characters.Player.Utils;
using HoloJam.Characters.Player;

namespace HoloJam
{
    public class Grabber : MonoBehaviour
    {
        [SerializeField]
        private Transform grabTransform;
        private Grabbable currentGrabbedObject;

        private Player mPlayer;
        private PlayerInput mInput;
        private Interactor mInteractor;
        private CharacterAnimation charAnimator;
        void Start()
        {
            mPlayer = GetComponent<Player>();
            mInput = mPlayer.Input;
            mInteractor = GetComponent<Interactor>();
            charAnimator = GetComponent<CharacterAnimation>();
        }
        public void SetGrabObject(Grabbable grabbedObject)
        {
            if (mPlayer.performingAction) return;
            if (currentGrabbedObject != null) return;
            mPlayer.PerformAction("pickup");

            mInteractor.HandsFree = false;
            grabbedObject.OnGrabbed();
            currentGrabbedObject = grabbedObject;
            charAnimator.SetPrefix("g");
        }

        // Update is called once per frame
        void Update()
        {
            if (currentGrabbedObject == null) return;
            UpdateGrabbedObjPosition();
            CheckRelease();
        }
        private void UpdateGrabbedObjPosition()
        {
            currentGrabbedObject.transform.position = grabTransform.position;
        }
        private void CheckRelease()
        {
            if (mPlayer.performingAction) return;
            if (mInput.GetInteractValue() == 0)
            {
                float moveInput = mInput.GetMovementInput();
                if (mInput.GetUpDownInput() > 0)
                {
                    currentGrabbedObject.ThrowUp(moveInput < 0);
                }
                else if (moveInput != 0)
                {
                    currentGrabbedObject.ThrowForward(moveInput < 0);
                }  else
                {
                    currentGrabbedObject.Drop();
                }
                currentGrabbedObject = null;
                mInteractor.HandsFree = true;
                charAnimator.SetPrefix("");
                charAnimator.PlayAnimation("idle");
            }
        }
    }
}
