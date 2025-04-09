using System;
using System.Linq;

using UnityEngine;

using System.Collections;

using Utilities;

namespace Entities {
    public class Health : MonoBehaviour {
        public float getPercentHealth => Mathf.Clamp01(currentHealth / maxHealth);
        public float getCurrentHealth => currentHealth;
        public float getMaxHealth => maxHealth;
        public bool isInvulnerable => invulnerable;
        public Action<float> onDamage;
        public Action<float> onHeal;
        public Action onDeath;
        public Action onInvulnerableDamage;

        private float invulnerabilityTimer = 0f;
        private bool invulnerable = false;
        private Coroutine invulnerabilityReset = null;

        [SerializeField] private float currentHealth;
        [SerializeField] private float maxHealth;

        private void Awake() {
            currentHealth = maxHealth;
        }

        private void UpdateMaxHealth(float health) {
            Debug.Log($"Updated max health for {name} to {health}");
            float diff = health - currentHealth;
            maxHealth = health;
            Heal(diff);
        }

        /// <summary>Damages an entity</summary>
        /// <param name="damage">Damage to apply</param>
        /// <param name="entityPos">Position of entity that applied the knockback</param>
        public void Damage(float damage, Vector2? entityPos = null) {
            if (currentHealth == 0.0f) { //Don't damage dead things!
                return;
            } else if (invulnerable) {
                onInvulnerableDamage?.Invoke();
                return;
            }
            damage = Mathf.Max(damage, 0.0f);
            if (damage != 0.0f) {
                currentHealth = Mathf.Max(0.0f, currentHealth - damage);
                onDamage?.Invoke(damage);
            }
            if (currentHealth == 0.0f) {
                Debug.Log($"{name} is dead");
                onDeath?.Invoke();
            }
        }

        public void Heal(float amount) {
            amount = Mathf.Max(0f, amount);
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            onHeal?.Invoke(amount);
        }

        public void SetInvulnerable(float time) {
            if (invulnerabilityReset != null) {
                StopCoroutine(invulnerabilityReset);
            }
            invulnerabilityTimer += time;
            invulnerabilityReset = StartCoroutine(InvulnerabilityReset());
        }

        public void SetInvulnerable(bool invulnerable) {
            this.invulnerable = invulnerable;
        }

        private IEnumerator InvulnerabilityReset() {
            while (invulnerabilityTimer >= 0f) {
                invulnerabilityTimer -= Time.fixedDeltaTime;
                yield return Yielders.WaitForFixedUpdate;
            }
        }
    }
}