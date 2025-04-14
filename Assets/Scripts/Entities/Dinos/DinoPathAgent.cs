using UnityEngine;

using Utilities;

namespace Entities.Dinos {
    public class DinoPathAgent : MonoBehaviour {
        [SerializeField] private Transform[] _points;

        public void Init(Transform[] points, int startIndex = 0) {
            _points = points;
            _index = startIndex;
        }

        public float Speed = 0.0f;
        public bool Move = true;

        [SerializeField] private int _index = 0;
        [SerializeField] private Vector2 _target;
        private Rigidbody2D _rb2D;
        private SpriteRenderer _renderer;

        private void Start() {
            _rb2D = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _target = _rb2D.position;
        }

        private void FixedUpdate() {
            if (Move && _points != null) {
                _target = TravelPath();
                _renderer.flipX = Mathf.Sign(_target.x - _rb2D.position.x) > 0;
                _rb2D.MovePosition(TravelPath());
            }
        }

        public Vector2 TravelPath() {
            if (Speed <= 0.0f) { return _rb2D.position; }
            if (Vector2.Distance(_points[_index].position, _rb2D.position) <= 0.01f && _index < _points.Length - 1) {
                _index++;
            }
            return Vector2.MoveTowards(_rb2D.position, _points[_index].position, Speed * Time.fixedDeltaTime);
        }
    }
}