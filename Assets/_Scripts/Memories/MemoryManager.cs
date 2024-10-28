using UnityEngine;
using System.Collections.Generic;
namespace HoloJam
{
    public class MemoryManager : MonoBehaviour
    {
        private static MemoryManager Instance;
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
        public static void SetVariable(string ID)
        {
            if (!Instance.savedMemoryIDs.Contains(ID))
            {
                Instance.savedMemoryIDs.Add(ID);
            }
        }
        public static bool HasVariable(string ID)
        {
            return Instance.savedMemoryIDs.Contains(ID);
        }
        public static void ClearSave()
        {
            Instance.savedMemoryIDs.Clear();
        }
    }
}
