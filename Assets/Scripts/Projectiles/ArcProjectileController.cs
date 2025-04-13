using System.Linq;

using Entities;

using UnityEngine;

namespace ProjectileComponents {
    public class ArcProjectileController : MonoBehaviour {

        [SerializeField] private float _speed;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector2 _linearPosition;
        [SerializeField] private Vector2 _initialPosition;
        [SerializeField] private Vector2 _targetPosition;
        [SerializeField] private float _progress = 0f;
        [SerializeField] private float _height;
        [SerializeField] private Rigidbody2D _rb2D;

        [SerializeField] private bool _move = false;

        private void Awake() {
            _rb2D = GetComponent<Rigidbody2D>();
        }

        public void Init(float speed, float height, Transform target) {
            _speed = speed;
            _target = target;
            _height = height;
            _linearPosition = transform.position;
            _initialPosition = transform.position;
            _move = true;
        }

        public void FixedUpdate() {
            if (_target) {
                if (Vector2.Distance(_targetPosition, _target.position) >= 0.1f) {
                    _targetPosition = _target.position;
                    _speed += 2.0f * Vector2.Distance(_targetPosition, _target.position);
                }
            }
            if (_move) {
                _progress = Mathf.Clamp01(Vector2.Distance(_linearPosition, _targetPosition) / Vector2.Distance(_initialPosition, _targetPosition));
                _linearPosition = Vector2.MoveTowards(_linearPosition, _targetPosition, Time.fixedDeltaTime * _speed);
                _rb2D.MovePosition(_linearPosition + _height * Mathf.Sin(_progress * Mathf.PI) * Vector2.up);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            _move = false;
            Debug.Log("Move stopped by" + collider.name);
        }
    }
}