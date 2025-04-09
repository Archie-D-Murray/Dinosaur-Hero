using System.Linq;

using Entities;

using UnityEngine;

namespace Spider {
    public class BabySpider : MonoBehaviour {

        [SerializeField] private Vector3 initialPosition;
        [SerializeField] private Vector3 linearPosition;
        [SerializeField] private Transform target;
        [SerializeField] private Movement movement;
        [SerializeField] private float range = 4;
        [SerializeField] private float height = 4;
        [SerializeField] private float damage = 4;
        [SerializeField] private float speed = 4;
        [SerializeField] private LayerMask hit = 4;
        [SerializeField] private float progress = 0f;

        Transform[] points;
        int index = 0;

        private void Start() {
            hit = 1 << LayerMask.NameToLayer("Tower") | 1 << LayerMask.NameToLayer("End");
            movement = GetComponent<Movement>();
        }

        public void Init(Transform[] points, int index) {
            movement = GetComponent<Movement>();
            this.points = points;
            this.index = index;
            movement.Locations = points;
            movement.counter = index;
            movement.AllowMovement();
        }

        public void FixedUpdate() {
            if (!target) {
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, range, hit);
                if (targets.Length != 0) {
                    movement.enabled = false;
                    target = targets.First().transform;
                    linearPosition = transform.position;
                    initialPosition = transform.position;
                }
            } else {
                if (Vector2.Distance(target.position, transform.position) <= 0.1f) {
                    target.GetComponent<Health>().Damage(damage);
                    Destroy(gameObject);
                } else {
                    progress = Mathf.Clamp01(Vector2.Distance(linearPosition, target.position) / Vector2.Distance(initialPosition, target.position));
                    linearPosition = Vector3.MoveTowards(linearPosition, target.position, Time.fixedDeltaTime * speed);
                    transform.position = linearPosition + height * Mathf.Sin(progress * Mathf.PI) * Vector3.up;
                }
            }
        }
    }
}