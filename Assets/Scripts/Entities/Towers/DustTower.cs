using ProjectileComponents;

using UnityEngine;

using Utilities;

namespace Entities.Towers {
    public class DustTower : Tower {
        [SerializeField] private float _stunDuration;

        public override void Shoot(Collider2D[] colliders) {
            _attackTimer.Reset(_attackTime);
            GameObject projectile = Instantiate(_projectile, transform.position, Helpers.Look2D(transform.position, GetClosest(colliders).transform.position));
            projectile.GetOrAddComponent<LinearProjectileMover>().Speed = _projectileSpeed;
            projectile.GetOrAddComponent<DustStorm>().StunDuration = _stunDuration;
            projectile.GetOrAddComponent<AutoDestroy>().Duration = _projectileDuration;
        }
    }
}