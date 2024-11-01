using UnityEngine;
using UnityEngine.SceneManagement;
using HoloJam.Dialogue.Data;
namespace HoloJam.Dialogue
{
    public class DialogueHitbox : MonoBehaviour
    {
        public string ignoreIfHasSaveID = "";
        public string customSaveID = "";
        public bool disableMove = true;
        public bool disableJump = true;
        public DialogueNode StartingNode;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }
        private string GetSaveID()
        {
            if (customSaveID != "")
            {
                return customSaveID;
            }
            return "D-" + SceneManager.GetActiveScene().name;
        }
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (ignoreIfHasSaveID != "" && MemoryManager.HasVariable(ignoreIfHasSaveID))
                {
                    return;
                }
                if (MemoryManager.HasVariable(GetSaveID()))
                {
                    return;
                }
                DialogueManager.Instance.RegisterDialogueEndEvent(SaveID);
                DialogueManager.Instance.StartDialogue(StartingNode, disableMove, disableJump);
            }
        }
        void SaveID()
        {
            DialogueManager.Instance.DeRegisterDialogueEndEvent(SaveID);
            MemoryManager.SetVariable(GetSaveID());
        }
        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                DialogueManager.Instance.EndDialogue();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.0f, 0.0f, 0.5f, 0.4f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
