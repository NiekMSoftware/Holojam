using UnityEngine;

namespace HoloJam.Characters.Data
{
    [System.Serializable]
    public class GroundedData
    {
        [field: SerializeField, Range(0f, 2f)] public float Acceleration { get; private set; }
        [field: SerializeField, Range(0f, 0.9f)] public float GroundDecay { get; private set; } 
        [field: SerializeField] public float MaxHorizontalSpeed { get; private set; }
    }
}
