using UnityEngine;

namespace HoloJam.Characters.Data
{
    [CreateAssetMenu(fileName = "Character", menuName = "Custom Characters/Character")]
    public class CharacterSO : ScriptableObject
    {
        [field: SerializeField] public GroundedData GroundedData { get; private set; }
        [field: SerializeField] public AirborneData AirborneData { get; private set; }
    }
}
