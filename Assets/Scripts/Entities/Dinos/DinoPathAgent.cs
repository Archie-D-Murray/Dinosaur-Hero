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

        private Rigidbody2D _rb2D;

        private void Start() {
            _rb2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            if (Move && _points != null) {
                _rb2D.MovePosition(TravelPath(ref _index, _rb2D.position, Speed * Helpers.NormalizedFixedDeltaTime));
            }
        }

        public Vector2 TravelPath(ref int index, Vector2 position, float speed) {
            if (speed <= 0.0f) { return position; }
            float sqrDistance = 0.0f;
            Vector2 next = position;
            while (sqrDistance <= speed * speed) {
                Vector2 target = _points[index].position;
                if (index == _points.Length - 1) {
                    next = Vector2.MoveTowards(position, target, speed);
                    break;
                } else {
                    next = Vector2.MoveTowards(position, target, speed);
                    if (index < _points.Length && Vector2.Distance(target, next) <= 0.1f) {
                        index++;
                    }
                    sqrDistance += (next - position).sqrMagnitude;
                }
            }

            return next;
        }
    }
}