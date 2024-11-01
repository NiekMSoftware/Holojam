using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;// Required when using Event data.
using HoloJam.Managers;
using static HoloJam.Managers.AudioManager;
using System;
using UnityEditor;
using TMPro;
namespace HoloJam
{
    public class PauseManager : MonoBehaviour
    {
        public static bool IsPaused { get { return Instance.isPaused; } }
        public static PauseManager Instance { get; private set; }
        [SerializeField]
        private Selectable mInitialButton;
        [SerializeField]
        private Animator pauseAnimator;
        [SerializeField]
        private EventSystem mEventSystem;
        [SerializeField]
        private TextMeshProUGUI typingSpeedText;
        private Selectable lastButtonSelected;
        private bool isPaused;

        private AudioLevels currentSfxLevel;
        private AudioLevels currentMusicLevel;
        private bool isFast;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // Initialize the levels from player prefs
            currentMusicLevel = (AudioLevels)PlayerPrefs.GetInt("MusicLevel", (int)AudioLevels.Medium);
            currentSfxLevel = (AudioLevels)PlayerPrefs.GetInt("SFXLevel", (int)AudioLevels.Medium);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (lastButtonSelected == null) lastButtonSelected = mInitialButton;
        }
        public static void TogglePause()
        {
            SetPause(!Instance.isPaused);
        }
        public static void SetPause(bool pauseValue)
        {
            Instance.isPaused = pauseValue;
            Time.timeScale = pauseValue ? 0 : 1;
            Instance.pauseAnimator.Play(pauseValue ? "pause": "unpause");
            if (pauseValue)
            {
                Instance.lastButtonSelected.Select();
            } else
            {
                Instance.lastButtonSelected = Instance.mEventSystem.currentSelectedGameObject.GetComponent<Selectable>();
                Instance.mEventSystem.SetSelectedGameObject(null);
            }
        }
        public void OnResume()
        {
            SetPause(false);
        }
        public void ToggleSFXVolume()
        {
            currentSfxLevel = GetNextAudioLevel(currentSfxLevel);
            AudioManager.Instance.SetVolume(currentSfxLevel, AudioMixers.Sfx);
        }
        public void ToggleMusicVolume()
        {
            currentMusicLevel = GetNextAudioLevel(currentMusicLevel);
            AudioManager.Instance.SetVolume(currentMusicLevel, AudioMixers.Music);
        }
        public void ToggleTypingSpeed()
        {
            isFast = !isFast;
            typingSpeedText.text = isFast ? "Typing Speed: Fast" : "Typing Speed: Slow";
            UIManager.Instance.SetTypingSpeed(isFast);
        }
        private AudioLevels GetNextAudioLevel(AudioLevels currentLevel)
        {
            int nextIndex = ((int)currentLevel + 1) % Enum.GetValues(typeof(AudioLevels)).Length;
            return (AudioLevels)nextIndex;
        }
        public void ReloadCurrentScene()
        {
            WorldManager.ReloadScene();
            SetPause(false);
        }
        public void ReturnToHub()
        {
            WorldManager.ReturnToHomeScene();
            SetPause(false);
        }
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
