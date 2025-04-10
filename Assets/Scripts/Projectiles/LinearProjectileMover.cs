using UnityEngine;

namespace ProjectileComponents {

    public class LinearProjectileMover : MonoBehaviour {

        public float Speed = 0f;
        private Vector3 _direction;

        private void Start() {
            _direction = transform.up;
        }

        private void FixedUpdate() {
            transform.position += Speed * Time.fixedDeltaTime * _direction;
        }
    }
}