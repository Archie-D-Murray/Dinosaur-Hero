using System;

using Entities;

using UnityEngine;
namespace ProjectileComponents {
    [Serializable] public enum DamageFilter { Tower, Enemy }
    public class EntityDamager : MonoBehaviour {
        [SerializeField] private float _damage;
        [SerializeField] private GameObject _source;

        public void Init(GameObject source, float damage) {
            _source = source;
            _damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent(out Health entityHealth)) {
                entityHealth.Damage(_damage, _source);
                // Instantiate(Assets.Instance.HitParticles, transform.position, Quaternion.LookRotation(-transform.up));
            }
        }
    }
}