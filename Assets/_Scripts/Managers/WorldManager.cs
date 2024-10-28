using UnityEngine;
using HoloJam.Characters.Player;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using TMPro;
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
        private float minY = -15f;
        private string areaName = "";
        private string roomName = "";
        [SerializeField]
        private Animator roomNameAnimator;
        [SerializeField]
        private TextMeshProUGUI bigZoneName;
        [SerializeField]
        private TextMeshProUGUI smallZoneName;
        [SerializeField]
        private TextMeshProUGUI smallRoomName;
        private TransitionTag lastTargetTag;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                FirstStart();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public static void SetYDeathZone(float DeathZone)
        {
            Instance.minY = DeathZone;
        }
        public static void UpdateRoomName(string areaName, string roomName)
        {
            if (Instance.areaName != areaName)
            {
                Instance.areaName = areaName;
                Instance.roomName = roomName;
                Instance.bigZoneName.text = areaName;
                Instance.smallZoneName.text = areaName;
                Instance.smallRoomName.text = areaName;
                if (areaName != "")
                {
                    Instance.roomNameAnimator.Play("zone", 0, 0);
                }
            } else if (Instance.roomName != roomName)
            {
                Instance.areaName = areaName;
                Instance.roomName = roomName;
                Instance.bigZoneName.text = areaName;
                Instance.smallZoneName.text = areaName;
                Instance.smallRoomName.text = roomName;
                if (roomName != "")
                {
                    Instance.roomNameAnimator.Play("room",0,0);
                }
            }
        }
        private void Update()
        {
            if (playerRef!= null && playerRef.transform.position.y < minY)
            {
                WorldManager.ReloadScene();
            }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void FirstStart()
        {
            homeSceneName = SceneManager.GetActiveScene().name;
            SetPlayer(FindObjectOfType<Player>());
            cachedPositionInHome = playerRef.transform.position;
            SceneManager.sceneLoaded += Instance.OnNewSceneLoaded;
        }
        public static void LoadMemoryScene(string sceneName)
        {
            Instance.lastTargetTag = TransitionTag.NONE;
            Instance.cachedPositionInHome = Instance.playerRef.transform.position;
            CorruptionManager.ResetEffects();
            SceneManager.LoadScene(sceneName);
        }
        public static void LoadNewHubScene(string scenename,TransitionTag originTag, TransitionTag targetTag)
        {
            if (targetTag == TransitionTag.AUTO)
            {
                switch(originTag)
                {
                    case TransitionTag.LEFT:
                        targetTag = TransitionTag.RIGHT;
                        break;
                    case TransitionTag.RIGHT:
                        targetTag = TransitionTag.LEFT;
                        break;
                    case TransitionTag.UP:
                        targetTag = TransitionTag.DOWN;
                        break;
                    case TransitionTag.DOWN:
                        targetTag = TransitionTag.UP;
                        break;
                    case TransitionTag.LEFT_2:
                        targetTag = TransitionTag.RIGHT_2;
                        break;
                    case TransitionTag.RIGHT_2:
                        targetTag = TransitionTag.LEFT_2;
                        break;
                    case TransitionTag.UP_2:
                        targetTag = TransitionTag.DOWN_2;
                        break;
                    case TransitionTag.DOWN_2:
                        targetTag = TransitionTag.UP_2;
                        break;
                    default:
                        break;
                }
            }
            Instance.lastTargetTag = targetTag;
            CorruptionManager.ResetEffects();
            Instance.homeSceneName = scenename;
            SceneManager.LoadScene(scenename);
        }
        public static void LoadNewHubScene(string sceneName ,Vector3 positionInWorld)
        {
            Instance.lastTargetTag = TransitionTag.NONE;
            CorruptionManager.ResetEffects();
            Instance.homeSceneName = sceneName;
            Instance.cachedPositionInHome = positionInWorld;
            SceneManager.LoadScene(sceneName);
        }
        public static void LoadNewScene(string sceneName)
        {
            Instance.lastTargetTag= TransitionTag.NONE;
            CorruptionManager.ResetEffects();
            SceneManager.LoadScene(sceneName);
        }
        public static void ReloadScene()
        {
            CorruptionManager.ResetEffects();
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
            if (lastTargetTag == TransitionTag.NONE)
            {
                playerRef.transform.position = cachedPositionInHome;
            }
            TransitionHitbox[] hbs = FindObjectsOfType<TransitionHitbox>();
            foreach(TransitionHitbox hb in hbs)
            {
                if (hb.myTag == lastTargetTag)
                {
                    Instance.cachedPositionInHome =  hb.SetPosition(playerRef);
                    return;
                }
            }
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
