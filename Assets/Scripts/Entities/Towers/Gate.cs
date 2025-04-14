using UI;

using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private GameObject _healthBarPrefab;

        [SerializeField] private GameObject[] _armourDestructionEffects;

        private Health _health;
        private EffectHandler _handler;
        private SpriteRenderer _renderer;
        private Material _material;
        private Collider2D _collider;
        private Image _healthBar;
        private SFXEmitter _emitter;

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
            _emitter = GetComponent<SFXEmitter>();

            GameObject healthBar = Instantiate(_healthBarPrefab, UIManager.Instance.WorldCanvas);
            healthBar.transform.SetPositionAndRotation(transform.position + 1.5f * Vector3.up, Quaternion.identity);
            _healthBar = healthBar.GetComponentInChildren<Tags.UI.ReadoutTag>().GetComponent<Image>();
            if (_healthBar) {
                _healthBar.fillAmount = _health.GetPercentHealth;
                Debug.Log($"Inited healthbar: {_healthBar.name}");
            }
        }

        private void OnDamage(float damage, GameObject source) {
            if (_health.GetPercentHealth <= _armourDestruction[_currentMitigationIndex]) {
                Instantiate(_armourDestructionEffects[_currentMitigationIndex], transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                _currentMitigationIndex = Mathf.Min(_currentMitigationIndex + 1, _armourDestruction.Length - 1);
                _health.DamageMultiplier = _damageMitigation[_currentMitigationIndex];
            }
            _emitter.Play(SoundEffectType.Hit);
            _renderer.FlashDamage(Assets.Instance.DamageFlash, _material, 0.25f, this);
            if (_healthBar) {
                _healthBar.fillAmount = _health.GetPercentHealth;
            }
        }

        private void OnDeath() {
            _collider.enabled = false;
            TowerManager.Instance.DisableAllTowers();
            GameManager.Instance.OnWin?.Invoke();
            GameManager.Instance.ReturnLiveDinos();
            Destroy(_healthBar.transform.parent.gameObject);
            Destroy(gameObject, 0.5f);
        }
    }
}