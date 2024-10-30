using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class TimedSwitch : IReactToOnOffToggle
    {

        public BlockType mBlockType;
        [SerializeField]
        private bool currentToggle;
        [SerializeField]
        private float timerValue;
        [SerializeField]
        private Sprite onSprite;
        [SerializeField]
        private Sprite offSprite;

        private SwitchInteractable switchInteractable;
        private SpriteRenderer mSprite;
        private float returnBackTime;
        private void Start()
        {
            switchInteractable = GetComponentInChildren<SwitchInteractable>();
            mSprite = GetComponentInChildren<SpriteRenderer>();
            switchInteractable.toggleEvent += ToggleEvent;
            //OnOffBlockManager.RegisterToggleObject(this, mBlockType);
        }
        private void OnDestroy()
        {
            switchInteractable.toggleEvent -= ToggleEvent;
        }
        void Update()
        {
            if (!currentToggle)
            {
                if (Time.timeSinceLevelLoad > returnBackTime)
                {
                    OnOffBlockManager.SetBlocksStatus(true, mBlockType);
                }
            }
        }
        void ToggleEvent(Player p)
        {
            if (!currentToggle) return;
            currentToggle = !currentToggle;
            returnBackTime = Time.timeSinceLevelLoad + timerValue;
            OnOffBlockManager.SetBlocksStatus(currentToggle, mBlockType);
            p.PerformAction("pickup");
        }
        public void SetToggle(bool toggle)
        {
            currentToggle = toggle;
            mSprite.sprite = toggle ? onSprite : offSprite;
        }

        public void OnToggle(bool toggleValue)
        {
            SetToggle(toggleValue);
        }
    }
}
