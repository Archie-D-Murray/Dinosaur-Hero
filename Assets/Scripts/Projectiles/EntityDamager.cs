using System;

using Entities;

using UnityEngine;
namespace ProjectileComponents {
    [Serializable] public enum DamageFilter { Tower, Enemy }
    public class EntityDamager : MonoBehaviour {
        public float Damage;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent(out Health entityHealth)) {
                entityHealth.Damage(Damage);
                // Instantiate(Assets.Instance.HitParticles, transform.position, Quaternion.LookRotation(-transform.up));
            }
        }
    }
}