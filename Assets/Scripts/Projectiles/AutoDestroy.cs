using UnityEngine;

namespace ProjectileComponents {

    [DefaultExecutionOrder(99)]
    public class AutoDestroy : MonoBehaviour {
        public float Duration;

        public void Start() {
            Destroy(gameObject, Duration);
        }

        public void OnTriggerEnter2D(Collider2D collider) {
            Destroy(gameObject);
        }
    }
}