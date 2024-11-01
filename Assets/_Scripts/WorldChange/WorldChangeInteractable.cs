using UnityEngine;
using HoloJam.Characters.Player;
using HoloJam.Characters.Player.Utils;
using UnityEngine.SceneManagement;
namespace HoloJam
{
    public class WorldChangeInteractable : Interactable
    {
        [SerializeField]
        private bool returnToHome;
        [SerializeField]
        private string sceneToLoad;
        [SerializeField]
        private GameObject specialOnParticle;
        [SerializeField]
        private GameObject specialUsedParticle;
        private void Start()
        {
            interactionType = InteractableType.EXIT;
            if (specialOnParticle != null)
            {
                if (MemoryManager.HasVariable(sceneToLoad))
                {
                    specialOnParticle.SetActive(false);
                    specialUsedParticle.SetActive(true);
                } else
                {
                    specialOnParticle.SetActive(true);
                    specialUsedParticle.SetActive(false);
                }
            }
        }
        public override void OnPerformInteraction(Player p)
        {
            if (returnToHome)
            {
                Debug.Log(SceneManager.GetActiveScene().name);
                MemoryManager.SetVariable(SceneManager.GetActiveScene().name);
                WorldManager.ReturnToHomeScene();
            }
            else
            {
                WorldManager.LoadMemoryScene(sceneToLoad);
            }
        }
        
    }
}
