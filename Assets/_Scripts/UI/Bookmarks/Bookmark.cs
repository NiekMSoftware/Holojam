using UnityEngine;
using UnityEngine.UI;
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

        private bool lastIsActive = true;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
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
        }
    }
}
