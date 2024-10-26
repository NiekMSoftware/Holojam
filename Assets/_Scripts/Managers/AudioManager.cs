using HoloJam.Audio;
using System;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Audio;

namespace HoloJam.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public enum AudioLevels
        {
            Muted,
            Low,
            Medium,
            High
        }

        public enum AudioMixers
        {
            Music,
            Sfx
        }

        private static AudioManager instance;
        public static AudioManager Instance => instance;

        public SoundData[] Sounds;
        [SerializeField] private AudioMixerGroup musicMixer;
        [SerializeField] private AudioMixerGroup sfxMixer;

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
            if (IsAudioLevelSaved())
            {
                UpdateVolume();
            }
            else
            {
                // Set default volume levels if not saved
                SetVolume(AudioLevels.Medium, AudioMixers.Music);
                SetVolume(AudioLevels.Medium, AudioMixers.Sfx);
            }

            Play(FirstSongToPlay);
        }

        #region Save and Load Audio
        private void OnEnable()
        {
            // update audio levels
            UpdateVolume();
        }

        private void UpdateVolume()
        {
            AudioLevels musicLevel = (AudioLevels)PlayerPrefs.GetFloat("Music", (int)AudioLevels.Medium);
            AudioLevels sfxLevel = (AudioLevels)PlayerPrefs.GetFloat("SFX", (int)AudioLevels.Medium);

            SetVolume(musicLevel, AudioMixers.Music);
            SetVolume(sfxLevel, AudioMixers.Sfx);
        }
        #endregion

        #region Playback Methods
        public void Play(string soundName)
        {
            SoundData sd = Array.Find(Sounds, sound => sound.name == soundName);
            if (sd == null) { Debug.LogWarning($"Sound: {soundName} was not found! Did you type it in correctly?"); return; }

            sd.Source.Play();
        }

        public void Stop(string soundName)
        {
            SoundData sd = Array.Find(Sounds, sound => sound.name == soundName);
            if (sd == null) { Debug.LogWarning($"Sound: {soundName} was not found! Did you type it in correctly?"); return; }

            sd.Source.Stop();
        }

        public void Pause(string soundName)
        {
            SoundData sd = Array.Find(Sounds, sound => sound.name == soundName);
            if (sd == null) { Debug.LogWarning($"Sound: {soundName} was not found! Did you type it in correctly?"); return; }

            sd.Source.Pause();
        }
        #endregion

        #region Settings
        public void SetVolume(AudioLevels level, AudioMixers mixers)
        {
            float volume;

            switch (level)
            {
                case AudioLevels.Muted:
                    volume = -80f;
                    break;

                case AudioLevels.Low:
                    volume = -54f;
                    break;

                case AudioLevels.Medium:
                    volume = -28f;
                    break;

                case AudioLevels.High:
                    volume = 0f;
                    break;
                default:
                    volume = 0f;
                    break;
            }

            switch (mixers)
            {
                case AudioMixers.Music:
                    musicMixer.audioMixer.SetFloat("Music", volume);
                    PlayerPrefs.SetFloat("Music", volume);
                    break;

                case AudioMixers.Sfx:
                    sfxMixer.audioMixer.SetFloat("SFX", volume);
                    PlayerPrefs.SetFloat("SFX", volume);
                    break;
            }

            PlayerPrefs.Save();
        }

        private bool IsAudioLevelSaved()
        {
            return PlayerPrefs.HasKey("Music") && PlayerPrefs.HasKey("SFX");
        }
        #endregion
    }
}
