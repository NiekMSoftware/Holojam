using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace HoloJam
{
    public class BookMarkSection : MonoBehaviour
    {
        [SerializeField]
        private Transform bookmarksTransform;
        [SerializeField]
        private Animator mAnimator;
        private Dictionary<CorruptionType, Bookmark> bookmarks = new Dictionary<CorruptionType, Bookmark>();
        [SerializeField]
        private Selectable defaultSelect;
        public Selectable lastSelection;

        private void Awake()
        {
            for (int i = 0;i < bookmarksTransform.childCount; i++)
            {
                Bookmark nextBM = bookmarksTransform.GetChild(i).GetComponent<Bookmark>();
                nextBM.parentSection = this;
                bookmarks[nextBM.corruptionType] = nextBM;
            }
        }
        public void SetPanelOpen(bool isOpen)
        {
            mAnimator.Play(isOpen ? "open" : "close");
            if (isOpen)
            {
                if (lastSelection == null)
                {
                    lastSelection = defaultSelect;
                }
                lastSelection.Select();
            }
        }
        
        public void SetBookmarkActive(CorruptionType cType, bool isActive)
        {
            bookmarks[cType].SetActive(isActive);
        }
        public void SetBookmarkQuantity(CorruptionType cType, int newQuantity)
        {
            bookmarks[cType].SetQuantity(newQuantity);
        }

    }
}
