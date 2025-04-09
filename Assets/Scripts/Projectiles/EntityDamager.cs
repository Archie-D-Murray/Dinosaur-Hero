using System;

using Entities;

using UnityEngine;
namespace ProjectileComponents {
    [Serializable] public enum DamageFilter { Tower, Enemy }
    public class EntityDamager : MonoBehaviour {
        [SerializeField] private DamageFilter _filter;
        [SerializeField] private float _damage;

        public void Init(DamageFilter filter, float damage) {
            _filter = filter;
            _damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            // Health entityHealth = _filter switch {
            //     DamageFilter.Tower => collision.gameObject.Has<TowerBase>() ? collision.GetComponent<Health>() : null,
            //     DamageFilter.Enemy => collision.gameObject.Has<Enemy>() ? collision.GetComponent<Health>() : null,
            //     _ => null
            // };

            if (collision.TryGetComponent(out Health entityHealth)) {
                entityHealth.Damage(_damage);
                // Instantiate(Assets.Instance.HitParticles, transform.position, Quaternion.LookRotation(-transform.up));
                Destroy(gameObject);
            }
        }
    }
}