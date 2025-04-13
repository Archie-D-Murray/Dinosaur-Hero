using ProjectileComponents;

using UnityEngine;

using Utilities;

namespace Entities.Towers {
    public class IceAgeTower : Tower {

        [SerializeField] private Effect _slow = Effect.Slow(0.6f, 2.5f);
        [SerializeField] private float _shotHeightMultiplier = 0.5f;

        protected override TowerAnimations InitAnimations() {
            return new TowerAnimations("Ice_Age");
        }

        protected override TowerType InitType() {
            return TowerType.Ice;
        }

        public override void Shoot(Collider2D[] colliders) {
            Collider2D target = colliders.Closest(transform.position);
            GameObject projectile = Instantiate(_projectile, transform.position, Helpers.Look2D(transform.position, target.transform.position));
            projectile.GetOrAddComponent<ArcProjectileController>().Init(_projectileSpeed, _range * _shotHeightMultiplier, target.transform);
            projectile.GetOrAddComponent<EffectApplicator>().Init(_slow);
            projectile.GetOrAddComponent<EntityDamager>().Init(gameObject, _damage);
            projectile.GetOrAddComponent<AutoDestroy>().Duration = _projectileDuration;
        }
    }
}