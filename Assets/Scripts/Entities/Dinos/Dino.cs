using UnityEngine;

using Entities;
using Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Entities.Dinos {

    public class DinoAnimations {
        public int Idle;
        public int Move;
        public int Attack;

        public DinoAnimations(string name) {
            Idle = Animator.StringToHash($"{name}_Idle");
            Move = Animator.StringToHash($"{name}_Move");
            Attack = Animator.StringToHash($"{name}_Attack");
        }
    }

    public enum DinoType { Rex, Ankylo, Stego }

    public abstract class Dino : MonoBehaviour {
        [SerializeField] protected float _attackRadius = 5.0f;
        [SerializeField] protected float _damage = 5.0f;
        [SerializeField] protected float _speed = 5.0f;
        [SerializeField] protected float _attackTime = 5.0f;
        [SerializeField] protected float _attackFrame = 0.5f;

        protected CountDownTimer _attackTimer = new CountDownTimer(0.0f);
        protected CountDownTimer _attackFrameTimer = new CountDownTimer(0.0f);

        protected bool _pendingAttackFrame;
        protected List<Timer> _timers = new List<Timer>();
        protected Health _health;
        protected Rigidbody2D _rb2D;
        protected DinoPathAgent _agent;
        protected EffectHandler _handler;
        protected int _currentAnimation;
        protected Animator _animator;

        protected virtual void Start() {
            _health = GetComponent<Health>();
            _rb2D = GetComponent<Rigidbody2D>();
            _handler = GetComponent<EffectHandler>();
            _agent = GetComponent<DinoPathAgent>();
            _animator = GetComponent<Animator>();
            _handler.Init(_health);
            AddTimer(_attackTimer);
            AddTimer(_attackFrameTimer);
            _attackTimer.OnTimerStop += ResetAnimatorSpeed;
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
        protected abstract DinoAnimations GetAnimations();

        private void FixedUpdate() {
            _agent.Speed = _speed * _handler.SpeedModifier;
            _agent.Move = !_handler.Stunned || !_attackTimer.IsRunning;
            foreach (Timer timer in _timers) {
                timer.Update(Time.fixedDeltaTime);
            }
            if (_attackTimer.IsFinished && GetTargets().Populated()) {
                _attackTimer.Reset(AttackTime());
                _attackFrameTimer.Reset(AttackTime() * _attackFrame);
                _animator.speed = _attackTime / AttackTime();
                _pendingAttackFrame = true;
            }
            if (_attackFrameTimer.IsFinished && _pendingAttackFrame) {
                Collider2D[] targets = GetTargets();
                if (targets.Populated()) {
                    Attack(targets);
                }
                _pendingAttackFrame = false;
            }
            if (_attackTimer.IsRunning) {
                SetAnimation(GetAnimations().Attack);
            } else if (_rb2D.velocity.sqrMagnitude > 0.01f) {
                SetAnimation(GetAnimations().Move);
            } else {
                SetAnimation(GetAnimations().Idle);
            }
        }

        protected void SetAnimation(int animation) {
            if (_currentAnimation == animation) { return; }
            _currentAnimation = animation;
            _animator.Play(_currentAnimation);
        }

        protected void ResetAnimatorSpeed() {
            _animator.speed = 1.0f;
        }
    }
}