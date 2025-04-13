using UnityEngine;

using System.Linq;
using System.Collections.Generic;
using Utilities;
using Entities.Dinos;
using System;
using TMPro;
using Tags.UI;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace UI {
    public class SpawnerUI : MonoBehaviour {

        [SerializeField] private int _index;
        [SerializeField] private DinoType[] _types;
        [SerializeField] private LayerMask _spawnerLayer;
        [SerializeField] private float _radius = 2.0f;
        [SerializeField] private CanvasFader _fader;

        [SerializeField] private Image _sprite;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _desc;
        [SerializeField] private Button _next;
        [SerializeField] private Button _prev;

        [SerializeField] private Button _close;

        private DinoType _type => _types[_index];
        private Dictionary<DinoType, DinoData> _lookup = new Dictionary<DinoType, DinoData>();

        private void Start() {
            _types = Enum.GetValues(typeof(DinoType)).Cast<DinoType>().ToArray();
            foreach (DinoData data in Assets.Instance.DinoData) {
                _lookup.Add(data.Type, data);
            }
            _spawnerLayer = 1 << LayerMask.NameToLayer("Spawner");
            _next.onClick.AddListener(Next);
            _prev.onClick.AddListener(Prev);
            UpdateUI();
        }

        private void Prev() {
            _index--;
            if (_index < 0) {
                _index = _types.Length - 1;
            }
            UpdateUI();
        }

        private void Next() {
            _index = ++_index % _types.Length;
            UpdateUI();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && Globals.Instance.Storage(_type).Count > 0) {
                Vector2 mousePos = Helpers.Instance.GetWorldMousePosition();
                Collider2D closestSpawner = Physics2D.OverlapCircleAll(mousePos, _radius, _spawnerLayer)?.Closest(mousePos) ?? null;

                if (closestSpawner != null && closestSpawner.TryGetComponent(out DinoSpawner spawner)) {
                    spawner.QueueSpawn(_type);
                    Globals.Instance.Storage(_type).Count--;
                }
            }
            if (Input.GetKeyDown(KeyCode.RightBracket)) {
                Next();
            }
            if (Input.GetKeyDown(KeyCode.LeftBracket)) {
                Prev();
            }
        }

        private void UpdateUI() {
            _name.text = _lookup[_type].Name;
            _sprite.sprite = _lookup[_type].Sprite;
            _desc.text = _lookup[_type].Description;
        }

        public void Toggle() {
            _fader.Toggle();
        }

        public void Hide() {
            _fader.Hide();
        }
    }
}