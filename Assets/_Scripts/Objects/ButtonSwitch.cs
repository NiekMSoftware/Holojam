using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class ButtonSwitch :  IReactToOnOffToggle
    {
        private Animator mAnimator; 

        private SwitchInteractable switchInteractable;
        private SpriteRenderer mSprite;
        private float lastCollide;
        private const float COLLIDE_GAP = 0.5f;
        [SerializeField]
        private bool heavySwitch = false;
        private void Start()
        {
            switchInteractable = GetComponentInChildren<SwitchInteractable>();
            mSprite = GetComponentInChildren<SpriteRenderer>();
            mAnimator = GetComponent<Animator>();
            switchInteractable.toggleEvent += ToggleEvent;
            //OnOffBlockManager.RegisterToggleObject(this, mBlockType);
        }
        private void OnDestroy()
        {
            if (switchInteractable != null)
            {
                switchInteractable.toggleEvent -= ToggleEvent;
            }
        }
        void ToggleEvent(Player p)
        {
            OnOffBlockManager.ToggleBlockStatus(mBlockType);
            mAnimator.Play("press",0,0);
            p.PerformAction("pickup");
        }
        public void SetToggle(bool toggle)
        {
            if (mAnimator != null)
            {
                mAnimator.Play("press", 0, 0);
            }
        }

        public override void OnToggle(bool toggleValue)
        {
            SetToggle(toggleValue);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody == null) return;
            if (heavySwitch && collision.attachedRigidbody.gameObject.CompareTag("specialHeavy"))
            {
                mAnimator.Update(0);
                lastCollide = Time.timeSinceLevelLoad;
                OnOffBlockManager.ToggleBlockStatus(mBlockType);
                return;
            }
            if (collision.isTrigger) return;
            if (collision.GetComponent<Hitbox>() != null) return;
            if (collision.attachedRigidbody.GetComponent<Player>() != null) return;
            if (mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f) return;
            if (Time.timeSinceLevelLoad - lastCollide < COLLIDE_GAP) return;
            if (heavySwitch && !collision.attachedRigidbody.gameObject.CompareTag("heavy")) return;
            mAnimator.Update(0);
            lastCollide = Time.timeSinceLevelLoad;
            OnOffBlockManager.ToggleBlockStatus(mBlockType);
        }
    }
}
