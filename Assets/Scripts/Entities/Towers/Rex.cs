using System;

using ProjectileComponents;

using UnityEngine;

using Utilities;

namespace Entities.Towers {

    public class Rex : Tower {
        [SerializeField] private float _stackMagnitude = 0.1f;
        [SerializeField] private float _stackDuration = 0.1f;
        [SerializeField] private int _maxStacks = 5;
        [SerializeField] private int _stacks = 0;

        private CountDownTimer _stackTimer = new CountDownTimer(0.0f);

        protected override void AddTimer() {
            _timers.Add(_stackTimer);
            _stackTimer.OnTimerStop += ResetStacks;
        }

        private void ResetStacks() {
            _stacks = 0;
        }

        public override void Shoot(Collider2D[] enemies) {
            _animator.Play(_fireID);
            if (_stackTimer.IsRunning) {
                _stacks = Mathf.Min(_stacks + 1, _maxStacks);
            }
            _attackTimer.Reset(_attackTime * 1.0f - (_stacks * _stackMagnitude));
            _stackTimer.Reset(_stackDuration);
            _attackTimer.Start();
            _emitter.Play(SoundEffectType.Shoot);
        }
    }
}