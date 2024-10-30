using UnityEngine;

namespace HoloJam
{
    public class OnOffActiveDeactiveResponder : IReactToOnOffToggle 
    {
        [SerializeField]
        public GameObject itemToDeactivate;
        [SerializeField]
        public GameObject itemToDeactivate2;
        [SerializeField]
        public Collider2D triggerTODeactivate;
        [SerializeField]
        public Collider2D triggerTODeactivate2;
        [SerializeField]
        public bool invert;
        private void Start()
        {
//            OnOffBlockManager.RegisterToggleObject(this, mBlockType);
        }
        public override void OnToggle(bool toggleValue)
        {
            if (invert)
            {
                itemToDeactivate.SetActive(!toggleValue);
                if (triggerTODeactivate != null)
                {
                    triggerTODeactivate.isTrigger = toggleValue;
                }
                if (triggerTODeactivate2 != null)
                {
                    triggerTODeactivate2.isTrigger = toggleValue;
                }
            } else
            {
                itemToDeactivate.SetActive(toggleValue);
                if (triggerTODeactivate != null)
                {
                    triggerTODeactivate.isTrigger = !toggleValue;
                }
                if (triggerTODeactivate2 != null)
                {
                    triggerTODeactivate2.isTrigger = toggleValue;
                }
            }
        }
    }
}
