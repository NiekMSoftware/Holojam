using UnityEngine;

namespace HoloJam
{
    public class WorldInfo : MonoBehaviour
    {
        public float YDeathZone = -20f;
        public string zoneName = "";
        public string roomName = "";

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            WorldManager.UpdateRoomName(zoneName, roomName);
            WorldManager.SetYDeathZone(YDeathZone);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
