using UnityEngine;
using HoloJam.Characters.Player;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
namespace HoloJam
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        [SerializeField]
        private CinemachineCamera cineCamera;
        private string homeSceneName;
        private Vector3 cachedPositionInHome;
        private Player playerRef;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            homeSceneName = SceneManager.GetActiveScene().name;
            SetPlayer(FindObjectOfType<Player>());
        }
        public static void LoadMemoryScene(string sceneName)
        {
            Instance.cachedPositionInHome = Instance.playerRef.transform.position;
            CorruptionManager.ResetEffects();
            SceneManager.sceneLoaded += Instance.OnMemorySceneLoaded;
            SceneManager.LoadScene(sceneName);
        }
        public static void ReturnToHomeScene()
        {
            if (Instance.homeSceneName == SceneManager.GetActiveScene().name) return;
            CorruptionManager.ResetEffects();
            SceneManager.sceneLoaded += Instance.OnHomeSceneLoaded;
            SceneManager.LoadScene(Instance.homeSceneName);
        }

        void OnHomeSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= Instance.OnHomeSceneLoaded;
            SetPlayer(FindObjectOfType<Player>());
            playerRef.transform.position = cachedPositionInHome;
        }
        void OnMemorySceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= Instance.OnMemorySceneLoaded;
            SetPlayer(FindObjectOfType<Player>());
        }

        private void SetPlayer(Player newPlayer)
        {
            if (newPlayer == null) return;
            cineCamera.Target.TrackingTarget = newPlayer.transform;
            playerRef = newPlayer;
        }
    }
}
