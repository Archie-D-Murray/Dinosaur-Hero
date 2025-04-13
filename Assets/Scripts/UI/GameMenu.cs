using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Utilities;

namespace UI {

    public class Scenes {
        public const int MainMenu = 0;
        public const int LevelOne = 1;
        public const int MaxLevel = 2;
    }
    public class GameMenu : MonoBehaviour {
        [SerializeField] private int _buildIndex;
        [SerializeField] private CanvasFader _lose;
        [SerializeField] private CanvasFader _win;

        private void Start() {
            _buildIndex = SceneManager.GetActiveScene().buildIndex;
            GameManager.Instance.OnWin += _win.Show;
            GameManager.Instance.OnLose += _lose.Show;
        }

        public void MainMenu() {
            FadeScreen.Instance.Black();
            StartCoroutine(ChangeSceneDelayed(Scenes.MainMenu));
        }

        public void NextLevel() {
            FadeScreen.Instance.Black();
            if (_buildIndex >= Scenes.MaxLevel) {
                StartCoroutine(ChangeSceneDelayed(Scenes.MainMenu));
            } else {
                StartCoroutine(ChangeSceneDelayed(_buildIndex + 1));
            }
        }

        public void ReloadLevel() {
            FadeScreen.Instance.Black();
            GameManager.Instance.Reset();
            StartCoroutine(ChangeSceneDelayed(_buildIndex));
        }

        private IEnumerator ChangeSceneDelayed(int sceneID) {
            yield return Yielders.WaitForSeconds(FadeScreen.Instance.MaxFadeTime);
            SceneManager.LoadScene(sceneID);
        }
    }
}