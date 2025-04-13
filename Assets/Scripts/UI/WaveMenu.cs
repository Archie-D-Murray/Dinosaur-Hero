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

namespace UI {
    public class WaveMenu : MonoBehaviour {

        struct UISlot {
            public TMP_Text Count;
            public Image Highlight;
        }

        [SerializeField] private int _index;
        [SerializeField] private DinoType[] _types;
        [SerializeField] private LayerMask _spawnerLayer;
        [SerializeField] private float _radius = 2.0f;
        [SerializeField] private GameObject _storageSlot;
        [SerializeField] private CanvasFader _fader;
        [SerializeField] private Transform _storageRoot;

        private Dictionary<DinoType, UISlot> _storage = new Dictionary<DinoType, UISlot>();
        private DinoType _type => _types[_index];

        private void Start() {
            _types = Enum.GetValues(typeof(DinoType)).Cast<DinoType>().ToArray();
            _spawnerLayer = 1 << LayerMask.NameToLayer("Spawner");
            foreach (DinoData data in Assets.Instance.DinoData) {
                GameObject storageSlot = Instantiate(_storageSlot, _storageRoot);
                UISlot uiSlot = new UISlot();
                TMP_Text counter = storageSlot.GetComponentInChildren<TMP_Text>();
                counter.text = $"x{Globals.Instance.Storage(data.Type).Count}";
                uiSlot.Count = counter;
                foreach (Image image in storageSlot.GetComponentsInChildren<Image>()) {
                    if (image.gameObject.HasComponent<IconTag>()) {
                        image.sprite = data.Icon;
                    }
                    if (image.gameObject.HasComponent<HighlightTag>()) {
                        uiSlot.Highlight = image;
                        image.color = image.color.WithAlpha(0.0f);
                    }
                }
                int index = Array.IndexOf(_types, data.Type);
                storageSlot.GetComponentInChildren<Button>().onClick.AddListener(() => SetType(index));
                _storage.Add(data.Type, uiSlot);
            }
            HighlightSlot();
        }

        private void SetType(int index) {
            _index = index;
            HighlightSlot();
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
            if (Input.GetKeyDown(KeyCode.RightBracket)) {
                _index = ++_index % _types.Length;
                HighlightSlot();
            }
            if (Input.GetKeyDown(KeyCode.LeftBracket)) {
                _index--;
                if (_index < 0) {
                    _index = _types.Length - 1;
                }
                HighlightSlot();
            }
        }

        private void HighlightSlot() {
            foreach (KeyValuePair<DinoType, UISlot> kvp in _storage) {
                kvp.Value.Highlight.color = kvp.Value.Highlight.color.WithAlpha(kvp.Key == _type ? 1.0f : 0.0f);
            }
        }
    }
}