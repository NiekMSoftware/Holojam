using UnityEngine;
using UnityEngine.Audio;

namespace HoloJam.Audio
{
    [CreateAssetMenu(fileName = "Sound Data", menuName = "Audio/SoundData")]
    public class SoundData : ScriptableObject
    {
        [field: SerializeField] public AudioClip Audioclip { get; private set; }
        [field: SerializeField] public AudioMixerGroup AudioGroup { get; private set; }
        public AudioSource Source { get; set; }

        [field: Space]
        [field: SerializeField, Range(0f, 1f)] public float Volume { get; set; }
        [field: SerializeField] public bool Looped { get; private set; }
    }
}
