using System;

using UnityEngine;

public class EntityAoESlow : MonoBehaviour {

    [SerializeField] private float _speedModifier;
    [SerializeField] private float _duration;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _filter;

    public void Start() {
        _filter = 1 << LayerMask.NameToLayer("Enemy");
    }

    public void Init(float speedModifier, float duration, float radius) {
        _speedModifier = speedModifier;
        _duration = duration;
        _radius = radius;
    }

    public void FixedUpdate() {
        foreach (Collider2D entity in Physics2D.OverlapCircleAll(transform.position, _radius, _filter)) {
            if (!entity.TryGetComponent(out Movement movement)) { return; }
            movement.ApplySlow(_speedModifier, _duration);
        }
    }
}