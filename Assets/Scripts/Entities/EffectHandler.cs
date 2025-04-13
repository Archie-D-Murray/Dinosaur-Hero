using UnityEngine;

using System.Linq;
using System.Collections.Generic;
using Unity.Collections;
using System;
using Utilities;

namespace Entities {
    public class EffectHandler : MonoBehaviour {

        static EffectType[] _types = Enum.GetValues(typeof(EffectType)).Cast<EffectType>().ToArray();

        [Serializable]
        class ParticleManager {
            public EffectType Type;
            public ParticleSystem Particles;
            public CountDownTimer Timer;
            public bool IsPlaying;

            public ParticleManager(EffectType type, ParticleSystem particles) {
                Type = type;
                Particles = particles;
                Timer = new CountDownTimer(0.0f);
                IsPlaying = false;

                Timer.OnTimerStart += Particles.Play;
                Timer.OnTimerStop += Particles.Stop;
            }
        }

        [SerializeField] private ParticleManager[] _particleManagers;

        private Dictionary<EffectType, ParticleManager> _lookup = new Dictionary<EffectType, ParticleManager>();
        private Health _health;

        [SerializeField] private List<Effect> _effects = new List<Effect>();

        public Health Health => _health;
        public bool Stunned = false;
        public float SpeedModifier = 1.0f;

        public void Init(Health health) {
            _health = health;
            _particleManagers = new ParticleManager[Assets.Instance.EffectParticleCount];
            foreach (EffectType type in _types) {
                ParticleSystem particles = Instantiate(Assets.Instance.GetEffectParticles(type), transform).GetComponent<ParticleSystem>();
                _particleManagers[(int)type] = new ParticleManager(type, particles);
                _lookup.Add(type, _particleManagers[(int)type]);
            }
        }

        public void ApplyEffect(Effect effect) {
            _effects.Add(effect);
            _effects.Last().Init();
            if (_lookup[effect.Type].Timer.RemainingTime < effect.Duration) {
                _lookup[effect.Type].Timer.Reset(effect.Duration);
            }
        }

        private void FixedUpdate() {
            Stunned = false;
            SpeedModifier = 1.0f;

            foreach (ParticleManager manager in _particleManagers) {
                manager.Timer.Update(Time.fixedDeltaTime);
            }

            for (int i = 0; i < _effects.Count;) {
                _effects[i].Update(Time.fixedDeltaTime, this);
                if (_effects[i].Finished()) {
                    _effects.RemoveAtSwapBack(i);
                } else {
                    i++;
                }
            }
        }
    }
}