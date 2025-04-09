using System;

using Entities;
using Entities.Towers;

using UnityEngine;
namespace ProjectileComponents {
    public class AoEEntityDamager : MonoBehaviour {
        [SerializeField] private DamageFilter _filter;
        [SerializeField] private float _damage;
        [SerializeField] private float _range;
        [SerializeField] private LayerMask _tower;

        public void Init(DamageFilter filter, float damage, float range, LayerMask tower) {
            _filter = filter;
            _damage = damage;
            _range = range;
            _tower = tower;
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
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, _range, _tower)) {
                if (!collider.TryGetComponent(out Health health)) {
                    continue;
                }
                health.Damage(_damage);
            }
        }
    }
}