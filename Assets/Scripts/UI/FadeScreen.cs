using Utilities;

using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace UI {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeScreen : Singleton<FadeScreen> {

        CanvasGroup _canvas;
        [SerializeField] private float _fadeSpeed = 2.0f;

        public float MaxFadeTime => 1.0f / _fadeSpeed;

        protected override void OnAutoCreate() {
            transform.parent = FindObjectsOfType<Canvas>().First(canvas => canvas.renderMode != RenderMode.WorldSpace).transform;
            gameObject.GetOrAddComponent<Image>().color = Color.black.WithAlpha(0.0f);
        }

        private void Start() {
            _canvas = GetComponent<CanvasGroup>();
            Clear();
        }

        public void Black() {
            _canvas.FadeCanvas(_fadeSpeed, false, this);
        }

        public void Clear() {
            _canvas.FadeCanvas(_fadeSpeed, true, this);
        }
    }
}