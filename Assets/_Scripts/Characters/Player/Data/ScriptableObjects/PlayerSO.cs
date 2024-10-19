using UnityEngine;

namespace HoloJam.Player.Data
{
    [CreateAssetMenu(fileName = "Player", menuName = "Custom Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field: Header("Horizontal Movement Properties")]
        [field: SerializeField, Range(0.1f, 2f)] public float Acceleration { get; private set; }
        [field: SerializeField] public float MaxHorizontalSpeed { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float GroundDecay { get; private set; }
    }
}
