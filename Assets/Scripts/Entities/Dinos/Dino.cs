using UnityEngine;

using Entities;

namespace Entities.Dinos {

    public enum DinoType { Ankylo, Stego, Rex }
    public abstract class Dino : MonoBehaviour {
        [SerializeField] protected float _attackRadius = 5.0f;
        [SerializeField] protected float _damage = 5.0f;
        [SerializeField] protected float _speed = 5.0f;
        [SerializeField] protected int _index;

        protected Health _health;
        protected Rigidbody2D _rb2D;
        protected DinoPathAgent _agent;
        protected EffectHandler _handler;

        private void Start() {
            _health = GetComponent<Health>();
            _rb2D = GetComponent<Rigidbody2D>();
            _handler = GetComponent<EffectHandler>();
            _agent = GetComponent<DinoPathAgent>();
        }

        protected virtual Collider2D[] GetTargets() {
            return Physics2D.OverlapCircleAll(_rb2D.position, _attackRadius, Globals.Instance.TowerLayer);
        }

        protected abstract void Attack(Collider2D[] target);

        private void FixedUpdate() {
            _agent.Speed = _speed * _handler.SpeedModifier;
            _agent.Move = !_handler.Stunned;
        }
    }
}