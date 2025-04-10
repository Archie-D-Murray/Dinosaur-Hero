using UnityEngine;

using Entities;

using System.Collections.Generic;

namespace ProjectileComponents {
    public class DustStorm : MonoBehaviour {
        private HashSet<EffectHandler> _effected = new HashSet<EffectHandler>();

        public float StunDuration;

        private void OnTriggerEnter2D(Collider2D collider) {
            if (collider.TryGetComponent(out EffectHandler effectHandler)) {
                if (_effected.Contains(effectHandler)) { return; }
                _effected.Add(effectHandler);
                effectHandler.AcceptEffect(Effect.Stun(StunDuration));
            }
        }

        private void OnTriggerExit2D(Collider2D collider) {
            if (collider.TryGetComponent(out EffectHandler effectHandler)) {
                _effected.Remove(effectHandler);
            }
        }
    }
}