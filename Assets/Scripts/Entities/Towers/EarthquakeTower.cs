using ProjectileComponents;

using UnityEngine;

using Utilities;

namespace Entities.Towers {
    public class EarthquakeTower : Tower {

        [SerializeField] private Effect _slow = Effect.Slow(0.6f, 2.5f);
        [SerializeField] private Effect _stun = Effect.Stun(1.0f);

        protected override TowerAnimations InitAnimations() {
            return new TowerAnimations("Earthquake");
        }

        public override void Shoot(Collider2D[] colliders) {
            Collider2D target = colliders.Closest(transform.position);
            GameObject projectile = Instantiate(_projectile, target.transform.position, Quaternion.identity);
            projectile.GetOrAddComponent<EffectApplicator>().Init(_slow, _stun);
            projectile.GetOrAddComponent<EntityDamager>().Init(gameObject, _damage);
            projectile.GetOrAddComponent<AutoDestroy>().Duration = _projectileDuration;
        }
    }
}