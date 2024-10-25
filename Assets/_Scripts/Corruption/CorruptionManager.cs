using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using HoloJam.Characters.Player;
using UnityEngine.EventSystems;// Required when using Event data.
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

        public static void TogglePanelOpen()
        {
            if (!Instance.effects.Values.Any(obj => obj.CanBeUsed)) return;
            Instance.isOpen = !Instance.isOpen;
            Time.timeScale = Instance.isOpen ? 0 : 1;
            Instance.bookmarkUI.SetPanelOpen(Instance.isOpen);
            if (Instance.isOpen)
                Instance.player.Input.EnableUIControls();
            else
            {
                Instance.player.Input.DisableUIControls();
                Instance.mEventSystem.SetSelectedGameObject(null);
            }
                
        }
        public static void RegisterEffect(CorruptionEffect effect, CorruptionType cType)
        {
            Instance.effects[cType] = effect;
            Instance.UpdateBookmarkUI(Instance.effects[cType]);
        }
        public static void SetEffectActive(CorruptionType cType, bool isActive)
        {
            if (Instance.effects.ContainsKey(cType))
            {
                Instance.effects[cType].CanBeUsed = isActive;
                Instance.UpdateBookmarkUI(Instance.effects[cType]);
            }
        }
        public static bool ModifyCharges(CorruptionType cType, int delta)
        {
            if (Instance.effects.ContainsKey(cType) && Instance.effects[cType].CanBeUsed)
            {
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
        }
        public static bool AttemptUseEffect(CorruptionType cType)
        {
            if (Instance.effects.ContainsKey(cType))
            {
                if (Instance.effects[cType].AttemptUse())
                {
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
                Instance.corruptableObjects.Remove(obj);
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
