using UnityEngine;

namespace Entities.Towers {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(EffectHandler))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]

    public class Gate : MonoBehaviour {

        [SerializeField] private float _maxHealth = 100.0f;
        [SerializeField] private float[] _armourDestruction = new float[] { 0.5f, 0.0f };
        [SerializeField] private float[] _damageMitigation = new float[] { 0.9f, 1.0f };
        [SerializeField] private int _currentMitigationIndex = 0;

        [SerializeField] private GameObject[] _armourDestructionEffects;

        private Health _health;
        private EffectHandler _handler;
        protected SpriteRenderer _renderer;
        protected Material _material;
        protected Collider2D _collider;

        private void Start() {
            _health = GetComponent<Health>();
            _renderer = GetComponent<SpriteRenderer>();
            _material = _renderer.material;
            _health.OnDamage += OnDamage;
            _health.OnDeath += OnDeath;
            _health.DamageMultiplier = _damageMitigation[_currentMitigationIndex];
            _health.SetMaxHealth(_maxHealth);
            _handler = GetComponent<EffectHandler>();
            _handler.Init(_health);
            _collider = GetComponent<Collider2D>();
        }

        private void OnDamage(float damage, GameObject source) {
            if (_health.GetPercentHealth <= _armourDestruction[_currentMitigationIndex]) {
                Instantiate(_armourDestructionEffects[_currentMitigationIndex], transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                _currentMitigationIndex = Mathf.Min(_currentMitigationIndex + 1, _armourDestruction.Length - 1);
                _health.DamageMultiplier = _damageMitigation[_currentMitigationIndex];
            }
            _renderer.FlashDamage(Assets.Instance.DamageFlash, _material, 0.25f, this);
        }

        private void OnDeath() {
            _collider.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}