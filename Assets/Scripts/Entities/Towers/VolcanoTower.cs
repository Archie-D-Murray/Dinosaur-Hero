using ProjectileComponents;

using UnityEngine;

using Utilities;

namespace Entities.Towers {
    public class VolcanoTower : Tower {

        [SerializeField] private Effect _fireDot = Effect.DoT(10.0f, 2.0f, 0.5f);

        public override void Shoot(Collider2D[] colliders) {
            Collider2D target = colliders.Closest(transform.position);
            GameObject projectile = Instantiate(_projectile, transform.position, Helpers.Look2D(transform.position, target.transform.position));
            projectile.GetOrAddComponent<ArcProjectileController>().Init(_projectileSpeed, _range * 0.1f, target.transform);
            projectile.GetOrAddComponent<EffectApplicator>().Init(_fireDot);
            projectile.GetOrAddComponent<EntityDamager>().Damage = _damage;
            projectile.GetOrAddComponent<AutoDestroy>().Duration = _projectileDuration;
        }
    }
}