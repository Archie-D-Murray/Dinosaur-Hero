using UnityEngine;

using System.Linq;
using System.Collections.Generic;
using Unity.Collections;
using System;

namespace Entities {
    public class EffectHandler : MonoBehaviour {
        private Health _health;

        [SerializeField] private List<Effect> _effects = new List<Effect>();

        public Health Health => _health;
        public bool Stunned = false;
        public float SpeedModifier = 1.0f;

        public void Init(Health health) {
            _health = health;
        }

        public void AcceptEffect(Effect effect) {
            _effects.Add(effect);
            _effects.Last().Init();
        }

        private void FixedUpdate() {
            Stunned = false;
            SpeedModifier = 1.0f;

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