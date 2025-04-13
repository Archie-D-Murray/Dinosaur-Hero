using System;

using UnityEngine;

using Utilities;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFader : MonoBehaviour {

        private Coroutine _fade = null;

        [SerializeField] private float _fadeSpeed = 2.0f;
        [SerializeField] private bool _showOnStart = false;
        [SerializeField] private bool _fadeOnStart = false;
        private CanvasGroup _canvas;
        public float Alpha => _canvas.alpha;

        private void Awake() {
            _canvas = GetComponent<CanvasGroup>();
            if (_fadeOnStart) {
                if (_showOnStart) {
                    _canvas.alpha = 0.0f;
                }
                _fade = _canvas.FadeCanvasC(_fadeSpeed, !_showOnStart, this);
            } else {
                _canvas.alpha = _showOnStart ? 1.0f : 0.0f;
                _canvas.interactable = _showOnStart;
                _canvas.blocksRaycasts = _showOnStart;
            }
        }

        public void Show() {
            if (_fade != null) {
                StopCoroutine(_fade);
            }
            _fade = _canvas.FadeCanvasC(_fadeSpeed, false, this);
        }

        public void Hide() {
            if (_fade != null) {
                StopCoroutine(_fade);
            }
            _fade = _canvas.FadeCanvasC(_fadeSpeed, true, this);
        }

        public void Toggle() {
            if (_canvas.alpha == 0.0f) {
                Show();
            } else if (_canvas.alpha == 1.0f) {
                Hide();
            }
        }
    }
}