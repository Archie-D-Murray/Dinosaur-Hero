using UnityEngine;

using Entities;
using Utilities;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Entities.Dinos {

    public class DinoAnimations {
        public int Idle;
        public int Move;
        public int Attack;
        public int Death;

        public DinoAnimations(string name) {
            Idle = Animator.StringToHash($"{name}_Idle");
            Move = Animator.StringToHash($"{name}_Move");
            Attack = Animator.StringToHash($"{name}_Attack");
            Death = Animator.StringToHash($"{name}_Death");
        }
    }

    public enum DinoType { Rex, Trike, Stego }

    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(EffectHandler))]
    [RequireComponent(typeof(DinoPathAgent))]
    public abstract class Dino : MonoBehaviour {
        [SerializeField] protected float _maxHealth = 100.0f;
        [SerializeField] protected float _attackRadius = 5.0f;
        [SerializeField] protected float _damage = 5.0f;
        [SerializeField] protected float _speed = 5.0f;
        [SerializeField] protected float _attackTime = 5.0f;
        [SerializeField] protected float _attackFrame = 0.5f;
        [SerializeField] protected float _attackAnimationLength = 0.0f;
        [SerializeField] protected float _animatorSpeed = 1.0f;
        [SerializeField] protected CountDownTimer _attackTimer = new CountDownTimer(0.0f);
        [SerializeField] protected CountDownTimer _attackFrameTimer = new CountDownTimer(0.0f);

        protected bool _pendingAttackFrame;
        protected List<Timer> _timers = new List<Timer>();
        protected Health _health;
        protected Rigidbody2D _rb2D;
        protected DinoPathAgent _agent;
        protected DinoAnimations _animations;
        protected EffectHandler _handler;
        protected int _currentAnimation;
        protected Animator _animator;
        protected SpriteRenderer _renderer;
        protected Material _material;
        protected DinoType _type;

        public DinoType Type => _type;

        protected virtual void Start() {
            _type = InitType();
            _health = GetComponent<Health>();
            _rb2D = GetComponent<Rigidbody2D>();
            _handler = GetComponent<EffectHandler>();
            _agent = GetComponent<DinoPathAgent>();
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _material = _renderer.material;
            _animations = InitAnimations();
            _handler.Init(_health);
            _health.SetMaxHealth(_maxHealth);
            _health.OnDamage += OnDamage;
            _health.OnDeath += OnDeath;
            AddTimer(_attackTimer);
            AddTimer(_attackFrameTimer);
            _attackTimer.OnTimerStop += ResetAnimatorSpeed;
            _attackAnimationLength = _animator.GetRuntimeClip(_animations.Attack).length;
            GameManager.Instance.RegisterDino(this);
        }

        private void OnDeath() {
            GameManager.Instance.UnregisterDino(this);
            gameObject.layer = 0;
            _animator.Play(_animations.Death);
            Destroy(gameObject, _animator.GetRuntimeClip(_animations.Death).length + 0.1f);
        }

        private void OnDamage(float damage, GameObject source) {
            _renderer.FlashDamage(Assets.Instance.DamageFlash, _material, 0.25f, this);
            Instantiate(Assets.Instance.DinoHitParticles, transform.position, Quaternion.identity);
        }

        protected virtual Collider2D[] GetTargets() {
            return Physics2D.OverlapCircleAll(_rb2D.position, _attackRadius, Globals.Instance.TowerLayer);
        }

        protected void AddTimer(Timer timer) {
            _timers.Add(timer);
        }

        protected virtual float AttackTime() {
            return _attackTime;
        }

        ///<summary>Called upon attack frame</summary>
        ///<param name="target">Array of enemies to attack => assumed not empty or null</param>
        protected abstract void Attack(Collider2D[] target);
        protected abstract DinoAnimations InitAnimations();
        protected abstract DinoType InitType();

        private void FixedUpdate() {
            if (_health.Dead) { return; }
            _agent.Speed = _speed * _handler.SpeedModifier;
            _agent.Move = !_handler.Stunned && !_attackTimer.IsRunning;
            foreach (Timer timer in _timers) {
                timer.Update(Time.fixedDeltaTime);
            }
            if (_attackTimer.IsFinished && GetTargets().Populated()) {
                _attackTimer.Reset(AttackTime());
                _attackFrameTimer.Reset(AttackTime() * _attackFrame);
                _animatorSpeed = _attackAnimationLength / AttackTime();
                _pendingAttackFrame = true;
            }
            if (_attackFrameTimer.IsFinished && _pendingAttackFrame) {
                Collider2D[] targets = GetTargets();
                if (targets.Populated()) {
                    Attack(targets);
                }
                _pendingAttackFrame = false;
            }
            if (_attackTimer.IsRunning && !_attackTimer.IsFinished) {
                SetAnimation(_animations.Attack);
            } else if (_rb2D.velocity.sqrMagnitude > 0.01f) {
                SetAnimation(_animations.Move);
            } else {
                SetAnimation(_animations.Idle);
            }
        }

        protected void SetAnimation(int animation) {
            if (_currentAnimation == animation) { return; }
            _currentAnimation = animation;
            _animator.Play(_currentAnimation);
            _animator.speed = _animatorSpeed;
        }

        protected void ResetAnimatorSpeed() {
            _animatorSpeed = 1.0f;
        }
    }
}