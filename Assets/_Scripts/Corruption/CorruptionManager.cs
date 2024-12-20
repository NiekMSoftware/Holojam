using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using HoloJam.Characters.Player;
using UnityEngine.EventSystems;// Required when using Event data.
using HoloJam.Managers;
using HoloJam.Characters.Player.Utils;
namespace HoloJam
{
    public class CorruptionManager : MonoBehaviour
    {
        public static CorruptionManager Instance;
        public BookMarkSection bookmarkUI;
        private List<CorruptableObject> corruptableObjects = new List<CorruptableObject>();
        private Dictionary<CorruptionType, CorruptionEffect> effects = new Dictionary<CorruptionType, CorruptionEffect>();
        public static bool IsOpen { get { return Instance.isOpen; } }
        private bool isOpen;
        [SerializeField] private Player player;
        [SerializeField] private EventSystem mEventSystem;
        [SerializeField] private Transform effectTransform;

        [SerializeField] private string sfxOpen;
        [SerializeField] private string sfxClose;
        [SerializeField] private string sfxRefresh;
        [SerializeField] private string sfxPlantDeath;
        [SerializeField] private string sfxSpellUnlock;
        private float timeSinceLastPlantDeath;
        private const float plantDeathGap = 0.5f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            if (player == null)
                player = FindFirstObjectByType<Player>();
            for (int i = 0; i < effectTransform.childCount; i++) {
                CorruptionEffect ce = effectTransform.GetChild(i).GetComponent<CorruptionEffect>();
                RegisterEffect(ce, ce.associatedType);
            }
        }
        private void Update()
        {
            if (PlayerInput.Instance == null) return;

            if (PlayerInput.Instance.GetCorruptionPressed() && PlayerInput.Instance.GetJumpValue() > 0 &&
                PlayerInput.Instance.GetInteractValue() > 0)
            {
                if (PlayerInput.Instance.GetUpDownInput() > 0 && PlayerInput.Instance.GetMovementInput() > 0) 
                {
                    ToggleEffectActive(CorruptionType.TIMESTOP);
                }
                if (PlayerInput.Instance.GetUpDownInput() > 0)
                {
                    ToggleEffectActive(CorruptionType.GRAVITY);
                } else if (PlayerInput.Instance.GetUpDownInput() < 0)
                {
                    ToggleEffectActive(CorruptionType.GLOBE);
                }
                else if (PlayerInput.Instance.GetMovementInput() > 0)
                {
                    ToggleEffectActive(CorruptionType.BIRD);
                }
                else if (PlayerInput.Instance.GetMovementInput() < 0)
                {
                    ToggleEffectActive(CorruptionType.KILL);
                }
            }
        }
        public static void TogglePanelOpen()
        {
            if (!Instance.effects.Values.Any(obj => obj.CanBeUsed)) return;
            SetPanelOpen(!Instance.isOpen);
        }
        public static void SetPanelOpen(bool isOpen)
        {
            if (isOpen == Instance.isOpen) return;
            Instance.isOpen = isOpen;
            Time.timeScale = Instance.isOpen ? 0 : 1;
            Instance.bookmarkUI.SetPanelOpen(Instance.isOpen);
            AudioManager.Instance.Play(Instance.isOpen ? Instance.sfxOpen : Instance.sfxClose);
            if (!Instance.isOpen)
            {
                Instance.mEventSystem.SetSelectedGameObject(null);
            }
        }
        public static void RegisterEffect(CorruptionEffect effect, CorruptionType cType)
        {
            Instance.effects[cType] = effect;
            Instance.UpdateBookmarkUI(Instance.effects[cType]);
        }
        public static void ToggleEffectActive(CorruptionType cType)
        {
            if (Instance.effects.ContainsKey(cType))
            {
                AudioManager.Instance.Play(Instance.sfxSpellUnlock);
                Instance.effects[cType].CanBeUsed = !Instance.effects[cType].CanBeUsed;
                Instance.UpdateBookmarkUI(Instance.effects[cType]);
            }
        }
        public static void SetEffectActive(CorruptionType cType, bool isActive)
        {
            if (Instance.effects.ContainsKey(cType))
            {
                AudioManager.Instance.Play(Instance.sfxSpellUnlock);
                Instance.effects[cType].CanBeUsed = isActive;
                Instance.UpdateBookmarkUI(Instance.effects[cType]);
            }
        }
        public static void PlayPlantDeathSound()
        {
            if (Time.timeSinceLevelLoad - Instance.timeSinceLastPlantDeath < plantDeathGap) return;

            Instance.timeSinceLastPlantDeath = Time.timeSinceLevelLoad;
        }
        public static bool ModifyCharges(CorruptionType cType, int delta)
        {
            if (Instance.effects.ContainsKey(cType) && Instance.effects[cType].CanBeUsed)
            {
                if (delta > 0)
                {
                    AudioManager.Instance.Play(Instance.sfxRefresh);
                }
                Instance.effects[cType].ModifyCharges(delta);
                Instance.UpdateBookmarkUI(Instance.effects[cType]);
                return true;
            }
            return false;
        }
        public static void ResetEffects()
        {
            foreach (CorruptionEffect effect in Instance.effects.Values)
            {
                effect.ResetCharges();
                Instance.UpdateBookmarkUI(effect);
            }
            CorruptionManager.SetPanelOpen(false);
        }
        public static bool AttemptUseEffect(CorruptionType cType)
        {
            if (Instance.effects.ContainsKey(cType))
            {
                if (Instance.effects[cType].AttemptUse())
                {
                    List<CorruptableObject> cleanedCorruptableObjs = new List<CorruptableObject>();
                    foreach (CorruptableObject obj in Instance.corruptableObjects)
                    {
                        if (obj != null)
                        {
                            cleanedCorruptableObjs.Add(obj);
                        }
                    }
                    Instance.corruptableObjects = cleanedCorruptableObjs;
                    Instance.corruptableObjects.ForEach(obj => obj.ReceiveEffect(cType));
                    ModifyCharges(cType, -1);
                    return true;
                }
                return false;
            } else
            {
                return false;
            }
        }

        public static void RegisterObject(CorruptableObject obj)
        {
            if (!Instance.corruptableObjects.Contains(obj))
            {
                Instance.corruptableObjects.Add(obj);
            }
            else
            {
                Debug.Log("Attempted to register already registered object: " + obj.gameObject.name);
            }
        }
        public static void DeregisterObject(CorruptableObject obj)
        {
            if (Instance.corruptableObjects.Contains(obj))
            {
                Instance.corruptableObjects.Remove(obj);
            } else
            {
                Debug.Log("Attempted to remove invalid corruptable object: " + obj.gameObject.name);
            }
        }
        private void UpdateBookmarkUI(CorruptionEffect effect)
        {
            bookmarkUI.SetBookmarkActive(effect.associatedType, effect.CanBeUsed);
            bookmarkUI.SetBookmarkQuantity(effect.associatedType, effect.currentCharges);
        }
    }
}
