using UnityEngine;
using System.Collections.Generic;
using HoloJam.Characters.Player.Utils;
namespace HoloJam
{
    public class MemoryManager : MonoBehaviour
    {
        private static MemoryManager Instance;
        public bool debugAllmemories;
        [SerializeField]
        private List<string> savedMemoryIDs = new List<string>();

        private void Awake()
        {
            // Check if instance already exists
            if (Instance != null && Instance != this)
            {
                // Destroy this instance if it is a duplicate
                Destroy(gameObject);
                return;
            }

            // Set the instance to this object
            Instance = this;

            // Optionally prevent destruction on scene load
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            if (PlayerInput.Instance == null) return;

            if (PlayerInput.Instance.GetPausePressed() && PlayerInput.Instance.GetJumpValue() > 0 && 
                PlayerInput.Instance.GetInteractValue() > 0 && PlayerInput.Instance.GetUpDownInput() > 0)
            {
                debugAllmemories = true;
            }
            if (PlayerInput.Instance.GetPausePressed() && PlayerInput.Instance.GetJumpValue() > 0 &&
                PlayerInput.Instance.GetInteractValue() > 0 && PlayerInput.Instance.GetUpDownInput() < 0)
            {
                debugAllmemories = false;
            }
        }
        public static void SetVariable(string ID)
        {
            if (!Instance.savedMemoryIDs.Contains(ID))
            {
                Instance.savedMemoryIDs.Add(ID);
            }
        }
        public static bool HasVariable(string ID)
        {
            if (Instance.debugAllmemories) return true;
            return Instance.savedMemoryIDs.Contains(ID);
        }
        public static void ClearSave()
        {
            Instance.savedMemoryIDs.Clear();
        }
    }
}
