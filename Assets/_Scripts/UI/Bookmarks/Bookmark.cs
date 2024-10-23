using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using TMPro;
namespace HoloJam
{
    public class Bookmark : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public CorruptionType corruptionType;
        [SerializeField]
        private Animator mAnimator;
        [SerializeField]
        private TextMeshProUGUI counter;
        [SerializeField]
        private Image bookmarkImage;
        [SerializeField]
        private Selectable selection;
        [SerializeField]
        private Color inactiveColor;
        [SerializeField]
        private Color activeColor;

        private bool lastIsActive = true;
        private bool lastHasCharges = true;
        [HideInInspector]
        public BookMarkSection parentSection;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }
        public void OnSelect(BaseEventData eventData)
        {
            //Debug.Log("selected: " + gameObject);
            parentSection.lastSelection = selection;
            mAnimator.Play("select");
        }

        public void OnDeselect(BaseEventData eventData)
        {
            //Debug.Log("deselected: " + gameObject);
            mAnimator.Play("idle");
        }

        public void OnUse()
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
