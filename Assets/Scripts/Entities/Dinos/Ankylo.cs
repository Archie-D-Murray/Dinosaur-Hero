using UnityEngine;

namespace Entities.Dinos {
    public class Ankylo : Dino {

        [SerializeField] protected float _consecutiveHitMultiplier = 0.9f;
        [SerializeField] protected Effect _stun = Effect.Stun(2.0f);

        protected override void Attack(Collider2D[] target) {
            float hitMultiplier = 1.0f;
            foreach (Collider2D hit in target) {
                if (hit.TryGetComponent(out Health health)) {
                    health.Damage(_damage * hitMultiplier, gameObject);
                    hitMultiplier *= _consecutiveHitMultiplier;
                }
                if (hit.TryGetComponent(out EffectHandler handler)) {
                    handler.ApplyEffect(_stun);
                }
            }
        }

        protected override DinoAnimations InitAnimations() {
            return new DinoAnimations("Ankylo");
        }
    }
}