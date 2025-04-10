using System.Collections.Generic;

using UnityEngine;

using Utilities;
using ProjectileComponents;
using System;

namespace Entities.Towers {

    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SFXEmitter))]
    public abstract class Tower : MonoBehaviour {
        [SerializeField] protected float _damage;
        [SerializeField] protected float _range;
        [SerializeField] protected Health _health;
        [SerializeField] protected CountDownTimer _attackTimer;
        [SerializeField] protected GameObject _projectile;
        [SerializeField] protected float _attackTime;
        [SerializeField] protected LayerMask _enemy;
        [SerializeField] protected bool _canShoot = true;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected SFXEmitter _emitter;
        [SerializeField] protected float _projectileDuration;
        [SerializeField] protected float _projectileSpeed;

        protected List<Timer> _timers = new List<Timer>();

        protected readonly int _fireID = Animator.StringToHash("Fire");

        public abstract void Shoot(Collider2D[] enemies);

        private void Start() {
            _attackTimer = new CountDownTimer(0f);
            _attackTimer.Start();
            _emitter = GetComponent<SFXEmitter>();
            _animator = GetComponentInChildren<Animator>();
            _health.OnDeath += () => {
                _canShoot = false;
                Instantiate(Assets.Instance.towerDeathParticles, transform.position, transform.rotation);
                Destroy(gameObject, _emitter.Length(SoundEffectType.Death));
                enabled = false;
            };
            _health.OnDamage += (_, _) => {
                _emitter.Play(SoundEffectType.Hit);
                Instantiate(Assets.Instance.towerHitParticles, transform.position, Quaternion.identity)
                    .GetOrAddComponent<AutoDestroy>().Duration = 1f;
            };
            _timers.Add(_attackTimer);
            AddTimer();
        }

        protected virtual void AddTimer() { }

        private void FixedUpdate() {
            foreach (Timer timer in _timers) {
                timer.Update(Time.fixedDeltaTime);
            }
            if (_attackTimer.IsFinished && _canShoot) {
                Collider2D[] targets = GetTargets();
                if (targets.Length > 0) {
                    Shoot(targets);
                }
            }
        }

        protected virtual Collider2D[] GetTargets() {
            return Physics2D.OverlapCircleAll(transform.position, _range, _enemy);
        }

        protected Collider2D GetClosest(Collider2D[] enemies) {
            Array.Sort(enemies, CompareDistance);
            return enemies[0];
        }

        private int CompareDistance(Collider2D enemy1, Collider2D enemy2) {
            float dist1 = Vector2.Distance(enemy1.transform.position, transform.position);
            float dist2 = Vector2.Distance(enemy2.transform.position, transform.position);
            if (dist1 > dist2) {
                return 1;
            }
            if (dist1 < dist2) {
                return -1;
            }
            return 0;
        }
    }
}