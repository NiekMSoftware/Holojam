using UnityEngine;

namespace HoloJam.Characters.Data
{
    [System.Serializable]
    public class AirborneData
    {
        [field: Header("Jumping Properties")]
        [field: SerializeField, Range(0.1f, 0.75f)] public float JumpTime { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float AddedJumpForce { get; private set; }
        [field: Space, SerializeField, Range(1.1f, 10f)] public float GravityMultiplier { get; set; }
        [field: SerializeField] public float MaxFallingSpeed { get; private set; }

        [field: Header("Coyote Jump Properties")]
        [field: SerializeField, Range(0.1f, 0.5f)] public float CoyoteTime { get; private set; } = 0.2f;
        [field: SerializeField] public bool Grounded { get; set; }
    }
}
