using HoloJam.Audio;
using System;
using UnityEngine;

namespace HoloJam.Managers
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance => instance;

        public SoundData[] Sounds;

        [Space]
        public string FirstSongToPlay;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            // initialize and pool all sounds
            foreach (var sound in Sounds)
            {
                GameObject go = new GameObject(sound.name);

                sound.Source = go.AddComponent<AudioSource>();
                sound.Source.clip = sound.Audioclip;

                sound.Source.volume = sound.Volume;
                sound.Source.outputAudioMixerGroup = sound.AudioGroup;
                sound.Source.loop = sound.Looped;

                go.transform.SetParent(gameObject.transform);
            }
        }

        private void Start()
        {
            Play(FirstSongToPlay);
        }

        private void Play(string soundName)
        {
            SoundData sd = Array.Find(Sounds, sound => sound.name == soundName);
            if (sd == null) { Debug.LogWarning($"Sound: {soundName} was not found! Did you type it in correctly?"); return; }

            sd.Source.Play();
        }
    }
}
