using Utilities;

namespace Entities {

    public enum EffectType { DoT, Slow, Stun }

    public class Effect {
        public float Magnitude = 1.0f;
        public float Duration = 1.0f;
        public float TickRate = 0.0f;
        public EffectType Type;

        private CountDownTimer _durationTimer;
        private CountDownTimer _tickTimer;

        public static Effect DoT(float tickDamage, float duration, float tickRate) {
            return new Effect(EffectType.DoT, tickDamage, duration, tickRate);
        }
        public static Effect Stun(float duration) {
            return new Effect(EffectType.Stun, 0.0f, duration, 0.0f);
        }
        public static Effect Slow(float speedModifier, float duration, float tickRate) {
            return new Effect(EffectType.DoT, speedModifier, duration, 0.0f);
        }

        private Effect(EffectType type, float magnitude, float duration, float tickRate) {
            Type = type;
            Magnitude = magnitude;
            Duration = duration;
            TickRate = tickRate;
        }

        public void Init() {
            _durationTimer = new CountDownTimer(Duration);
            _tickTimer = new CountDownTimer(0.0f);
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
            return _durationTimer.IsFinished;
        }
    }
}