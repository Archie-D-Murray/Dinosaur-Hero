using System.Collections;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Utilities;

namespace UI {
    public class MainMenu : MonoBehaviour {
        private const string MASTER_VOLUME = "MasterVolume";
        private const string BGM_VOLUME = "BGMVolume";
        private const string SFX_VOLUME = "SFXVolume";
        [SerializeField] private AudioMixer _mixer;

        private void Start() {
            Globals.Instance.InitStorage();
        }

        public void Play() {
            FadeScreen.Instance.Black();
            StartCoroutine(LoadSceneDelayed(Scenes.LevelOne));
        }

        public void MasterVolume(float value) {
            _mixer.SetFloat(MASTER_VOLUME, 20 * Mathf.Log10(value));
        }
        public void BGMVolume(float value) {
            _mixer.SetFloat(BGM_VOLUME, 20 * Mathf.Log10(value));
        }
        public void SFXVolume(float value) {
            _mixer.SetFloat(SFX_VOLUME, 20 * Mathf.Log10(value));
        }

        public void Quit() {
            FadeScreen.Instance.Black();
            StartCoroutine(QuitFade());
        }

        private IEnumerator LoadSceneDelayed(int sceneID) {
            yield return Yielders.WaitForSeconds(FadeScreen.Instance.MaxFadeTime);
            SceneManager.LoadScene(sceneID);
        }

        private IEnumerator QuitFade() {
            yield return Yielders.WaitForSeconds(FadeScreen.Instance.MaxFadeTime);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}