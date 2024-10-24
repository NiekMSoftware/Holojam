using UnityEngine;
using HoloJam.Characters.Player;
using HoloJam.Characters.Player.Utils;
using UnityEngine.SceneManagement;
namespace HoloJam
{
    public class WorldChangeInteractable : MonoBehaviour
    {
        [SerializeField]
        private bool returnToHome;
        [SerializeField]
        private string sceneToLoad;
        private PlayerInput input;
        private bool active;
        private void Update()
        {
            if (!active || input == null) return;
            if (input.GetInteractValue() > 0)
            {
                if (returnToHome)
                {
                    WorldManager.ReturnToHomeScene();
                }
                else
                {
                    WorldManager.LoadMemoryScene(sceneToLoad);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody.GetComponent<Player>() == null) return;
            
            // grab the input component once
            if (input == null) input = collision.GetComponentInParent<Player>().Input;
            active = true;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.attachedRigidbody.GetComponent<Player>() == null) return;
            active = false;
        }
    }
}
