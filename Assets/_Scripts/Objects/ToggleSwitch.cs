using UnityEngine;
using HoloJam.Characters.Player;

namespace HoloJam
{
    public class ToggleSwitch :  IReactToOnOffToggle 
    { 

        
        [SerializeField]
        private bool currentToggle;
        [SerializeField]
        private Sprite onSprite;
        [SerializeField]
        private Sprite offSprite;

        private SwitchInteractable switchInteractable;
        private SpriteRenderer mSprite;
        private void Start()
        {
            switchInteractable = GetComponentInChildren<SwitchInteractable>();
            mSprite = GetComponentInChildren<SpriteRenderer>();
            switchInteractable.toggleEvent += ToggleEvent;
            OnOffBlockManager.RegisterToggleObject(this, mBlockType);
        }
        private void OnDestroy()
        {
            switchInteractable.toggleEvent -= ToggleEvent;
        }
        void ToggleEvent(Player p)
        {
            currentToggle = !currentToggle;
            OnOffBlockManager.SetBlocksStatus(currentToggle, mBlockType);
            p.PerformAction("pickup");
        }
        public void SetToggle(bool toggle)
        {
            if (mSprite == null) return;
            currentToggle = toggle;
            mSprite.sprite = toggle ? onSprite : offSprite;
        }

        public override void OnToggle(bool toggleValue)
        {
            SetToggle(toggleValue);
        }
    }
}
