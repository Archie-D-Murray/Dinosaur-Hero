using UnityEngine;

namespace ProjectileComponents {

    public class AutoDestroy : MonoBehaviour {
        public float Duration;

        public void Start() {
            Destroy(gameObject, Duration);
        }

        public void OnTriggerEnter2D(Collider2D collider) {
            Destroy(collider, 0.1f);
        }
    }
}