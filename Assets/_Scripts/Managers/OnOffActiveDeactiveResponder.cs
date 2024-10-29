using UnityEngine;

namespace HoloJam
{
    public class OnOffActiveDeactiveResponder : MonoBehaviour, IReactToOnOffToggle
    {
        [SerializeField]
        public BlockType mBlockType;
        [SerializeField]
        public GameObject itemToDeactivate;
        [SerializeField]
        public Collider2D triggerTODeactivate;
        [SerializeField]
        public bool invert;
        private void Start()
        {
            OnOffBlockManager.RegisterToggleObject(this, mBlockType);
        }
        public void OnToggle(bool toggleValue)
        {
            if (invert)
            {
                itemToDeactivate.SetActive(!toggleValue);
                if (triggerTODeactivate != null)
                {
                    triggerTODeactivate.isTrigger = toggleValue;
                }
            } else
            {
                itemToDeactivate.SetActive(toggleValue);
                if (triggerTODeactivate != null)
                {
                    triggerTODeactivate.isTrigger = !toggleValue;
                }
            }
        }
    }
}
