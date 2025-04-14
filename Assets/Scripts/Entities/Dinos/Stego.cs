using System;

using UnityEngine;

using Utilities;

namespace Entities.Dinos {
    public class Stego : Dino {

        [SerializeField] private float _reflectMultiplier = 0.5f;
        [SerializeField] private float _damageMultiplier = 0.5f;

        private CountDownTimer _stackTimer = new CountDownTimer(0.0f);

        protected override void Start() {
            base.Start();
            _health.DamageMultiplier = _damageMultiplier;
            _health.OnDamage += ReflectDamage;
        }

        private void ReflectDamage(float damage, GameObject source) {
            if (!source) { return; }
            if (source.TryGetComponent(out Health health)) {
                health.Damage(damage * _reflectMultiplier, gameObject);
            }
        }

        protected override void Attack(Collider2D[] target) {
            if (target.Closest(_rb2D.position).TryGetComponent(out Health health)) {
                health.Damage(_damage, gameObject);
            }
        }

        protected override DinoAnimations InitAnimations() {
            return new DinoAnimations("Stego");
        }

        protected override DinoType InitType() {
            return DinoType.Stego;
        }
    }
}