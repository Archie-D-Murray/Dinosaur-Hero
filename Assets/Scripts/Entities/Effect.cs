using System;

using Utilities;

namespace Entities {

    public enum EffectType { DoT, Slow, Stun }

    [Serializable]
    public class Effect {
        public float Magnitude = 1.0f;
        public float Duration = 1.0f;
        public float TickRate = 0.0f;
        public EffectType Type;

        private bool _init = false;
        private CountDownTimer _durationTimer;
        private CountDownTimer _tickTimer;

        public static Effect DoT(float tickDamage, float duration, float tickRate) {
            return new Effect(EffectType.DoT, tickDamage, duration, tickRate);
        }
        public static Effect Stun(float duration) {
            return new Effect(EffectType.Stun, 0.0f, duration, 0.0f);
        }
        public static Effect Slow(float speedModifier, float duration) {
            return new Effect(EffectType.Slow, speedModifier, duration, 0.0f);
        }

        private Effect(EffectType type, float magnitude, float duration, float tickRate) {
            Type = type;
            Magnitude = magnitude;
            Duration = duration;
            TickRate = tickRate;
        }

        public void Init() {
            _durationTimer = new CountDownTimer(Duration);
            _tickTimer = new CountDownTimer(TickRate);
            _durationTimer.Start();
            _tickTimer.Start();
            _init = true;
        }

        public void Update(float dt, EffectHandler handler) {
            _durationTimer.Update(dt);
            switch (Type) {
                case EffectType.DoT:
                    _tickTimer.Update(dt);
                    if (_tickTimer.IsFinished && Type == EffectType.DoT) {
                        handler.Health.Damage(Magnitude);
                    }
                    break;

                case EffectType.Stun:
                    handler.Stunned = true;
                    break;

                case EffectType.Slow:
                    handler.SpeedModifier *= Magnitude;
                    break;
            }
        }

        public bool Finished() {
            if (!_init) { return false; }
            return _durationTimer.IsFinished;
        }

        public Effect Clone() {
            return Type switch {
                EffectType.DoT => Effect.DoT(Magnitude, Duration, TickRate),
                EffectType.Slow => Effect.Slow(Magnitude, Duration),
                _ => Effect.Stun(Duration),
            };
        }
    }
}