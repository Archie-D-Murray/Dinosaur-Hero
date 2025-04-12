using System;

using Entities;
using Entities.Towers;

using UnityEngine;
namespace ProjectileComponents {
    public class AoEEntityDamager : MonoBehaviour {
        [SerializeField] private float _damage;
        [SerializeField] private float _range;
        [SerializeField] private LayerMask _filter;

        public void Init(float damage, float range, LayerMask filter) {
            _damage = damage;
            _range = range;
            _filter = filter;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            // Health entityHealth = _filter switch {
            //     DamageFilter.Tower => collision.gameObject.Has<TowerBase>() ? collision.GetComponent<Health>() : null,
            //     DamageFilter.Enemy => collision.gameObject.Has<Enemy>() ? collision.GetComponent<Health>() : null,
            //     _ => null
            // };
            if (!collision.gameObject.HasComponent<Tower>()) {
                return;
            }
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, _range, _filter)) {
                if (!collider.TryGetComponent(out Health health)) {
                    continue;
                }
                health.Damage(_damage);
            }
        }
    }
}