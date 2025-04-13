using UnityEngine;

using Utilities;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFader : MonoBehaviour {
        [SerializeField] private float _fadeSpeed = 2.0f;
        [SerializeField] private bool _showOnStart = false;
        private CanvasGroup _canvas;

        private void Awake() {
            _canvas = GetComponent<CanvasGroup>();
            _canvas.alpha = _showOnStart ? 1.0f : 0.0f;
            _canvas.interactable = _showOnStart;
            _canvas.blocksRaycasts = _showOnStart;
        }

        public void Show() {
            _canvas.FadeCanvas(_fadeSpeed, false, this);
        }

        public void Hide() {
            _canvas.FadeCanvas(_fadeSpeed, true, this);
        }
    }
}