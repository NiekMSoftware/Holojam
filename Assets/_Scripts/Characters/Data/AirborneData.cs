using UnityEngine;

namespace HoloJam.Characters.Data
{
    [System.Serializable]
    public class AirborneData
    {
        [field: SerializeField] public float JumpingForce { get; private set; }
        public bool Grounded { get; set; }
    }
}
