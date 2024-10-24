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
        private void Start()
        {
            interactionType = InteractableType.EXIT;
        }
        public override void OnPerformInteraction(Player p)
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
}
