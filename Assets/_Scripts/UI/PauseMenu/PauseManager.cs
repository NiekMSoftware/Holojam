using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;// Required when using Event data.
using HoloJam.Managers;
using static HoloJam.Managers.AudioManager;
using System;
using UnityEditor;
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
        private Selectable lastButtonSelected;
        private bool isPaused;

        private AudioLevels currentSfxLevel = AudioLevels.Medium;
        private AudioLevels currentMusicLevel = AudioLevels.Medium;

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
            currentSfxLevel = (AudioLevels)(((int)currentSfxLevel + 1) % Enum.GetValues(typeof(AudioLevels)).Length);
            AudioManager.Instance.SetVolume(currentSfxLevel, AudioMixers.Sfx);
        }
        public void ToggleMusicVolume()
        {
            currentMusicLevel = (AudioLevels)(((int)currentMusicLevel + 1) % Enum.GetValues (typeof(AudioLevels)).Length);
            AudioManager.Instance.SetVolume(currentMusicLevel, AudioMixers.Music);
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
