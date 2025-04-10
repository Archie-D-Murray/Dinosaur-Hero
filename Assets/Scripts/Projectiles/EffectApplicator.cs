using System;

using Entities;

using UnityEngine;

namespace ProjectileComponents {
    public class EffectApplicator : MonoBehaviour {
        [SerializeField] private Effect[] _effects;

        public void Init(Effect[] effects) {
            _effects = new Effect[effects.Length];
            Array.Copy(effects, _effects, effects.Length);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            // Health entityHealth = _filter switch {
            //     DamageFilter.Tower => collision.gameObject.Has<TowerBase>() ? collision.GetComponent<Health>() : null,
            //     DamageFilter.Enemy => collision.gameObject.Has<Enemy>() ? collision.GetComponent<Health>() : null,
            //     _ => null
            // };

            if (collision.TryGetComponent(out EffectHandler handler)) {
                foreach (Effect effect in _effects) {
                    handler.AcceptEffect(effect);
                }
                // Instantiate(Assets.Instance.HitParticles, transform.position, Quaternion.LookRotation(-transform.up));
                Destroy(gameObject);
            }
        }
    }
}