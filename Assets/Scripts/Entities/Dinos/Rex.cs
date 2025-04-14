using System;

using UnityEngine;

using Utilities;

namespace Entities.Dinos {
    public class Rex : Dino {

        [SerializeField] private float _stackTime = 1.5f;
        [SerializeField] private float _attackSpeedMultiplier = 0.1f;
        [SerializeField] private int _maxStacks = 5;
        [SerializeField] private int _stacks = 0;

        private CountDownTimer _stackTimer = new CountDownTimer(0.0f);

        protected override void Start() {
            _animations = new DinoAnimations("Rex");
            base.Start();
            _stackTimer.OnTimerStop += ResetStacks;
            AddTimer(_stackTimer);
        }

        private void ResetStacks() {
            _stacks = 0;
        }

        protected override float AttackTime() {
            return _attackTime * (1.0f - _attackSpeedMultiplier * _stacks);
        }

        protected override void Attack(Collider2D[] target) {
            if (target.Closest(_rb2D.position).TryGetComponent(out Health health)) {
                health.Damage(_damage, gameObject);
                if (_stackTimer.IsRunning) {
                    _stacks = Mathf.Min(_maxStacks, _stacks + 1);
                }
                _stackTimer.Reset(_stackTime);
            }
        }

        protected override DinoAnimations InitAnimations() {
            return _animations;
        }

        protected override DinoType InitType() {
            return DinoType.Rex;
        }
    }
}