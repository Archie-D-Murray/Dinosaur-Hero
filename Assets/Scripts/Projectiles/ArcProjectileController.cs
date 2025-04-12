using System.Linq;

using Entities;

using UnityEngine;

namespace ProjectileComponents {
    public class ArcProjectileController : MonoBehaviour {

        [SerializeField] private float _speed;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _linearPosition;
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private float _progress = 0f;
        [SerializeField] private float _height;
        [SerializeField] private Rigidbody2D _rb2D;

        private void Awake() {
            _rb2D = GetComponent<Rigidbody2D>();
        }

        public void Init(float speed, float height, Transform target) {
            _speed = speed;
            _target = target;
            _height = height;
            _linearPosition = transform.position;
            _initialPosition = transform.position;
        }

        public void FixedUpdate() {
            if (_target) {
                _targetPosition = _target.position;
            }
            if (Vector2.Distance(_targetPosition, transform.position) <= 0.2f) {
                Destroy(gameObject);
            } else {
                // TODO: Fix this to use an arc
                _progress = Mathf.Clamp01(Vector2.Distance(_linearPosition, _targetPosition) / Vector2.Distance(_initialPosition, _targetPosition));
                _linearPosition = Vector3.MoveTowards(_linearPosition, _targetPosition, Time.fixedDeltaTime * _speed);
                _rb2D.MovePosition(_linearPosition + _height * Mathf.Sin(_progress * Mathf.PI) * Vector3.up);
            }
        }
    }
}