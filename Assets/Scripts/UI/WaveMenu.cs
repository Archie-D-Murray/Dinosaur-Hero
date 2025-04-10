using UnityEngine;

using System.Linq;
using System.Collections.Generic;
using Utilities;
using Entities.Dinos;
using System;

namespace UI {
    public class WaveMenu : MonoBehaviour {

        [SerializeField] private int _index;
        [SerializeField] private DinoType[] _types;
        [SerializeField] private LayerMask _spawnerLayer;
        [SerializeField] private float _radius = 2.0f;

        private DinoType _type => _types[_index];

        private void Start() {
            _types = Enum.GetValues(typeof(DinoType)).Cast<DinoType>().ToArray();
            _spawnerLayer = 1 << LayerMask.NameToLayer("Spawner");
        }

        private void Update() {
            if (Input.GetMouseButton(0) && Globals.Instance.Storage(_type).Count > 0) {
                Vector2 mousePos = Helpers.Instance.GetWorldMousePosition();
                Collider2D closestSpawner = Physics2D.OverlapCircleAll(mousePos, _radius, _spawnerLayer)?.Closest(mousePos) ?? null;

                if (closestSpawner != null && closestSpawner.TryGetComponent(out DinoSpawner spawner)) {
                    spawner.QueueSpawn(_type);
                    Globals.Instance.Storage(_type).Count--;
                }
            }
        }
    }
}