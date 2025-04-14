using UnityEngine;

namespace ProjectileComponents {

    [DefaultExecutionOrder(99)]
    public class AutoDestroy : MonoBehaviour {
        [SerializeField] private float _duration;
        [SerializeField] private bool _autoDestroy = false;

        public float Duration {
            get {
                return _duration;
            }
            set {
                _duration = value;
                Destroy(gameObject, _duration);
            }
        }

        public void Start() {
            if (_autoDestroy) {
                Destroy(gameObject, _duration);
            }
        }

        public void OnTriggerEnter2D(Collider2D collider) {
            Destroy(gameObject);
        }
    }
}