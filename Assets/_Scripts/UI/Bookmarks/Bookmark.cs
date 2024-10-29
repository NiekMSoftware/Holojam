using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using HoloJam.Characters.Player.Utils;
using TMPro;
namespace HoloJam
{
    public class Bookmark : MonoBehaviour
    {
        public CorruptionType corruptionType;
        [SerializeField]
        private Animator mAnimator;
        [SerializeField]
        private TextMeshProUGUI counter;
        [SerializeField]
        private Image bookmarkImage;
        [SerializeField]
        private Color inactiveColor;
        [SerializeField]
        private Color activeColor;

        [SerializeField]
        private Bookmark onSelectUp;
        [SerializeField]
        private Bookmark onSelectDown;

        private bool lastIsActive = true;
        private bool lastHasCharges = true;
        [HideInInspector]
        public BookMarkSection parentSection;
        [SerializeField]
        private bool isSelected;
        private bool toSelect;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }
        void Update()
        {
            if (!isSelected)
            {
                if (toSelect)
                {
                    isSelected = true;
                    toSelect = false;
                }
                return;
            } 
            if (PlayerInput.Instance.GetJumpPressed() || PlayerInput.Instance.GetInteractValue() > 0)
            {
                Use();
                return;
            }
            bool pressedThisFrame = PlayerInput.Instance.UpdownThisFrame();
            if (!pressedThisFrame) return;
            Bookmark bestSelectable;
            if (PlayerInput.Instance.GetUpDownInput() > 0)
            {
                bestSelectable = onSelectUp;
                while (!bestSelectable.gameObject.activeInHierarchy)
                {
                    bestSelectable = bestSelectable.onSelectUp;
                }
                if (bestSelectable != this)
                {
                    Dehighlight();
                }
                
                bestSelectable.Highlight();
            } else if (PlayerInput.Instance.GetUpDownInput() < 0)
            {
                bestSelectable = onSelectDown;
                while (!bestSelectable.gameObject.activeInHierarchy)
                {
                    bestSelectable = bestSelectable.onSelectDown;
                }
                if (bestSelectable != this)
                {
                    Dehighlight();
                }
                bestSelectable.Highlight();
            }
        }
        public void Highlight()
        {
            Debug.Log("selected: " + gameObject);
            mAnimator.Play("select");
            toSelect = true;
            parentSection.lastBookmark = this;
        }

        public void Dehighlight()
        {
            Debug.Log("deselected: " + gameObject);
            mAnimator.Play("idle");
            isSelected = false;
        }

        public void Use()
        {
            if (!lastHasCharges) return;
            mAnimator.Play("use");
            CorruptionManager.AttemptUseEffect(corruptionType);
            CorruptionManager.TogglePanelOpen();
        }
        public void SetActive(bool newActive)
        {
            if (lastIsActive == newActive) return;
            if (newActive)
            {
                gameObject.SetActive(true);
                mAnimator.Play("unlocked");
                
            } else
            {
                mAnimator.Play("hidden");
                gameObject.SetActive(false);
            }
            lastIsActive = newActive;
        }
        public void SetQuantity(int newQuantity)
        {
            if (newQuantity <= 1)
            {
                counter.text = "";
            } else
            {
                counter.text = newQuantity.ToString();
            }
            bookmarkImage.color = (newQuantity == 0) ? inactiveColor : activeColor;
            lastHasCharges = (newQuantity != 0);
        }
    }
}
