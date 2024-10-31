using UnityEngine;
using HoloJam.Characters.Player;

namespace HoloJam
{
    public class MemoryPaperBlock : MonoBehaviour
    {
        public string searchMemID1;
        public string searchMemID2;
        public string searchMemID3;
        public string searchMemID4;
        public string searchMemID5;

        private Animator mAnim;
        [SerializeField]
        private float triggerDistance = 7;
        private bool empty = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            mAnim = GetComponent<Animator>();
            if (MemoryManager.HasVariable(GenerateID()))
            {
                mAnim.Play("empty", 0, 0);
                empty = true;
            }
        }
        private string GenerateID()
        {
            return "PaperAnimPlayed-" + searchMemID1 + '-' + searchMemID2 + '-' + searchMemID3;
        }
        private bool TriggeredMemories()
        {
            if (searchMemID1 != "" && !MemoryManager.HasVariable(searchMemID1)) 
                return false;
            if (searchMemID2 != "" && !MemoryManager.HasVariable(searchMemID2))
                return false;
            if (searchMemID3 != "" && !MemoryManager.HasVariable(searchMemID3))
                return false;
            if (searchMemID4 != "" && !MemoryManager.HasVariable(searchMemID4))
                return false;
            if (searchMemID5 != "" && !MemoryManager.HasVariable(searchMemID5))
                return false;
            return true;
        }

        // Update is called once per frame
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (empty) return;
            if (!enabled) return;
            if (collision.attachedRigidbody == null) return;
            if (collision.attachedRigidbody.GetComponent<Player>() == null) return;
            if (TriggeredMemories())
            {
                mAnim.Play("fade", 0, 0);
                MemoryManager.SetVariable(GenerateID());
                empty = true;
            }
        }
    }
}
