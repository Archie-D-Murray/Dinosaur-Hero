using System.Collections.Generic;

using UnityEngine;

using Utilities;
using ProjectileComponents;
using System;
using System.Linq;

namespace Entities.Towers {

    public enum TowerType { Volcano, Earthquake, Dust, Ice }

    public class TowerAnimations {
        public int Idle;
        public int Attack;

        public TowerAnimations(string name) {
            Idle = Animator.StringToHash($"{name}_Idle");
            Attack = Animator.StringToHash($"{name}_Attack");
        }
    }

    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EffectHandler))]
    [RequireComponent(typeof(SFXEmitter))]
    public abstract class Tower : MonoBehaviour {
        [SerializeField] protected GameObject _projectile;
        [SerializeField] protected TowerType _type;
        [SerializeField] protected float _maxHealth = 10.0f;
        [SerializeField] protected float _damage = 1.0f;
        [SerializeField] protected float _range = 2.0f;
        [SerializeField] protected float _attackTime = 1.0f;
        [SerializeField] protected float _attackFrame = 0.5f;
        [SerializeField] protected float _attackAnimationLength = 1.0f;
        [SerializeField] protected float _projectileDuration = 4.0f;
        [SerializeField] protected float _projectileSpeed = 5.0f;
        [SerializeField] protected bool _canShoot = true;
        [SerializeField] protected bool _pendingAttackFrame = false;
        [SerializeField] protected int _reward = 50;
        [SerializeField] protected CountDownTimer _attackTimer = new CountDownTimer(0.0f);
        [SerializeField] protected CountDownTimer _attackFrameTimer = new CountDownTimer(0.0f);

        protected List<Timer> _timers = new List<Timer>();
        protected Health _health;
        protected Animator _animator;
        protected SFXEmitter _emitter;
        protected TowerAnimations _animations;
        protected Collider2D _collider;
        protected SpriteRenderer _renderer;
        protected Material _material;
        protected EffectHandler _handler;

        public TowerType Type => _type;

        public abstract void Shoot(Collider2D[] enemies);

        private void Start() {
            _type = InitType();
            _attackTimer = new CountDownTimer(0f);
            _attackTimer.Start();
            _emitter = GetComponent<SFXEmitter>();
            _animator = GetComponentInChildren<Animator>();
            _health = GetComponent<Health>();
            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _handler = GetComponent<EffectHandler>();
            _material = _renderer.material;
            _health.OnDeath += () => {
                TowerManager.Instance.OnTowerDestroy(this);
                Globals.Instance.ChangeMoney(_reward);
                _canShoot = false;
                _collider.enabled = false;
                gameObject.layer = 0;
                Destroy(gameObject);
                enabled = false;
            };
            _health.OnDamage += (_, _) => {
                _emitter.Play(SoundEffectType.Hit);
                _renderer.FlashDamage(Assets.Instance.DamageFlash, _material, 0.25f, this);
                Instantiate(Assets.Instance.GetTowerParticles(_type), transform.position, Quaternion.identity);
            };
            _health.SetMaxHealth(_maxHealth);
            _handler.Init(_health);
            _timers.Add(_attackTimer);
            _timers.Add(_attackFrameTimer);
            AddTimer();
            _animations = InitAnimations();
            _attackTimer.OnTimerStop += IdleAnimation;
            _attackAnimationLength = _animator.GetRuntimeClip(_animations.Attack).length;
            TowerManager.Instance.RegisterTower(this);
        }

        private void IdleAnimation() {
            _animator.Play(_animations.Idle);
            _animator.speed = 1.0f;
        }

        protected virtual void AddTimer() { }
        protected abstract TowerAnimations InitAnimations();
        protected abstract TowerType InitType();

        private void FixedUpdate() {
            if (!_canShoot) { return; }
            foreach (Timer timer in _timers) {
                timer.Update(Time.fixedDeltaTime);
            }
            if (_attackTimer.IsFinished && !_pendingAttackFrame) {
                Collider2D[] targets = GetTargets();
                if (targets.Populated()) {
                    _pendingAttackFrame = true;
                    _attackFrameTimer.Reset(_attackTime * _attackFrame);
                    _attackTimer.Reset(_attackTime);
                    _animator.Play(_animations.Attack);
                    _animator.speed = _attackAnimationLength / _attackTime;
                }
            }
            if (_attackFrameTimer.IsFinished && _pendingAttackFrame) {
                Collider2D[] targets = GetTargets();
                if (targets.Populated()) {
                    Shoot(targets);
                }
                _pendingAttackFrame = false;
            }
        }

        protected virtual Collider2D[] GetTargets() {
            return Physics2D.OverlapCircleAll(transform.position, _range, Globals.Instance.DinoLayer);
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

        public void DisableTower() {
            _canShoot = false;
        }
    }
}