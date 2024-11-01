using UnityEngine;
using HoloJam.Characters.Player.Utils;
using HoloJam.Managers;
namespace HoloJam
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput player;
        bool frozen = false;
        private Animator mAnimator;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            mAnimator = GetComponent<Animator>();
            if (!MemoryManager.HasVariable("title"))
            {
                frozen = true;
                Time.timeScale = 0;
            } else
            {
                mAnimator.Play("empty");
            }

           
        }

        // Update is called once per frame
        void Update()
        {
            if (!frozen) return;
            if (player == null)
            {
                Unfreeze();
            } else if (player.GetInteractionPressed() || player.GetJumpPressed()) {
                Unfreeze();
            }

        }
        void Unfreeze()
        {
            mAnimator.Play("start");
            Time.timeScale = 1;
            AudioManager.Instance.Play(WorldManager.Instance.hubSong);
            frozen = false;
            MemoryManager.SetVariable("title");
        }
    }
}
