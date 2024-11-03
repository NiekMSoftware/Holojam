using HoloJam.Audio;
using System;
using TMPro;
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

        [Space]
        public TMP_Text sfxBtnText;
        public TMP_Text mscBtnText;

        private AudioSource lastMusicSource;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
                

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
            UpdateVolume();
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
            // Retrieve saved levels, defaulting to Medium if no value is saved
            int savedMusicLevel = PlayerPrefs.GetInt("MusicLevel", (int)AudioLevels.Medium);
            int savedSFXLevel = PlayerPrefs.GetInt("SFXLevel", (int)AudioLevels.Medium);

            AudioLevels musicLevel = (AudioLevels)savedMusicLevel;
            AudioLevels sfxLevel = (AudioLevels)savedSFXLevel;

            // Apply the retrieved levels
            SetVolume(musicLevel, AudioMixers.Music);
            SetVolume(sfxLevel, AudioMixers.Sfx);
        }
        #endregion

        #region Playback Methods
        public void PlayMusicSwapSeamless(string songName)
        {
            if (songName == "") return;
            SoundData sd = Array.Find(Sounds, sound => sound.name == songName);
            if (sd == null) { Debug.LogWarning($"Sound: {songName} was not found! Did you type it in correctly?"); return; }
            float lastTime = 0;
            if (lastMusicSource != null)
            {
                lastTime = lastMusicSource.time;
                lastMusicSource.Stop();
            }
            sd.Source.Play();
            lastMusicSource = sd.Source;
            sd.Source.time = lastTime;
        }
        public void Play(string soundName)
        {
            if (soundName == "") return;
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

            if (mixers == AudioMixers.Sfx)
            {
                switch (level)
                {
                    case AudioLevels.Muted:
                        volume = -80f;
                        SetText(level, mixers);
                        break;

                    case AudioLevels.Low:
                        volume = -28f;
                        SetText(level, mixers);
                        break;

                    case AudioLevels.Medium:
                        volume = -16f;
                        SetText(level, mixers);
                        break;

                    case AudioLevels.High:
                        volume = -6f;
                        SetText(level, mixers);
                        break;
                    default:
                        volume = -16f;
                        SetText(level, mixers);
                        break;
                }
            } else
            {
                switch (level)
                {
                    case AudioLevels.Muted:
                        volume = -80f;
                        SetText(level, mixers);
                        break;

                    case AudioLevels.Low:
                        volume = -54f;
                        SetText(level, mixers);
                        break;

                    case AudioLevels.Medium:
                        volume = -28f;
                        SetText(level, mixers);
                        break;

                    case AudioLevels.High:
                        volume = -12f;
                        SetText(level, mixers);
                        break;
                    default:
                        volume = -12f;
                        SetText(level, mixers);
                        break;
                }
            }
            

            switch (mixers)
            {
                case AudioMixers.Music:
                    musicMixer.audioMixer.SetFloat("Music", volume);
                    PlayerPrefs.SetFloat("Music", volume);
                    PlayerPrefs.SetInt("MusicLevel", (int)level);
                    break;

                case AudioMixers.Sfx:
                    sfxMixer.audioMixer.SetFloat("SFX", volume);
                    PlayerPrefs.SetFloat("SFX", volume);
                    PlayerPrefs.SetInt("SFXLevel", (int)level);
                    break;
            }

            PlayerPrefs.Save();
        }

        private void SetText(AudioLevels levels, AudioMixers mixers)
        {
            switch (mixers)
            {
                case AudioMixers.Music:
                    mscBtnText.text = $"{mixers} Volume: {levels}";
                    break;

                case AudioMixers.Sfx:
                    sfxBtnText.text = $"{mixers} Volume: {levels}";
                    break;
            }
        }
        #endregion
    }
}
