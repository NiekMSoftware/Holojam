using UnityEngine;

namespace HoloJam
{
    public class GlobalManagers : MonoBehaviour
    {
        private static GlobalManagers instance;
        public static GlobalManagers Instance;

        private void Awake()
        {
            // Check if instance already exists
            if (instance != null && instance != this)
            {
                // Destroy this instance if it is a duplicate
                Destroy(gameObject);
                return;
            }

            // Set the instance to this object
            instance = this;

            // Optionally prevent destruction on scene load
            DontDestroyOnLoad(gameObject);
        }
    }
}
