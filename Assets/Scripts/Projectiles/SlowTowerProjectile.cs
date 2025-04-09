using System.Linq;

using Entities;

using UnityEngine;

namespace ProjectileComponents {
    public class SlowTowerProjectile : MonoBehaviour {

        [SerializeField] private float _speed;
        [SerializeField] private GameObject slowPrefab;
        [SerializeField] private Vector3 _linearPosition;
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private float _progress = 0f;
        [SerializeField] private LayerMask _filter;
        [SerializeField] private float _height;
        [SerializeField] private Rigidbody2D _rb2D;
        private float _speedModifier;
        private float _duration;
        private float _slowDuration;

        private void Awake() {
            _rb2D = GetComponent<Rigidbody2D>();
        }

        public void Init(float speed, float speedModifier, float slowDuration, float duration, float height, Vector3 target, LayerMask filter) {
            _speed = speed;
            _height = height;
            _linearPosition = transform.position;
            _initialPosition = transform.position;
            _targetPosition = target;
            _filter = filter;
            _speedModifier = speedModifier;
            _slowDuration = slowDuration;
            _duration = duration;
        }

        public void FixedUpdate() {
            if (Vector2.Distance(_targetPosition, transform.position) <= 0.2f) {
                GameObject slowField = Instantiate(slowPrefab, transform.position, Quaternion.identity);
                slowField.GetOrAddComponent<EntityAoESlow>().Init(_speedModifier, _duration, _height);
                slowField.GetOrAddComponent<AutoDestroy>().Duration = _duration;
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