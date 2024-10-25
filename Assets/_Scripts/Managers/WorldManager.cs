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
            cachedPositionInHome = playerRef.transform.position;
            SceneManager.sceneLoaded += Instance.OnNewSceneLoaded;
        }
        public static void LoadMemoryScene(string sceneName)
        {
            Instance.cachedPositionInHome = Instance.playerRef.transform.position;
            CorruptionManager.ResetEffects();
            SceneManager.LoadScene(sceneName);
        }
        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public static void ReturnToHomeScene()
        {
            if (Instance.homeSceneName == SceneManager.GetActiveScene().name) return;
            CorruptionManager.ResetEffects();
            SceneManager.LoadScene(Instance.homeSceneName);
        }
        void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Instance.homeSceneName == SceneManager.GetActiveScene().name)
            {
                OnHomeSceneLoaded(scene, mode);
            } else
            {
                OnMemorySceneLoaded(scene, mode);
            }
        }
        void OnHomeSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetPlayer(FindObjectOfType<Player>());
            playerRef.transform.position = cachedPositionInHome;
        }
        void OnMemorySceneLoaded(Scene scene, LoadSceneMode mode)
        {
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
