using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using Utilities;

namespace Entities.Dinos {

    [Serializable]
    public class DinoGroup {
        public DinoType Type;
        public int Count = 1;
    }

    public class DinoSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Transform[] _points;
        [SerializeField] float _spawnDelay = 1.0f;

        private SpriteRenderer _hightlight;
        private Dictionary<DinoType, DinoData> _lookup = new Dictionary<DinoType, DinoData>();
        private Queue<DinoType> _spawnQueue = new Queue<DinoType>();
        private CountDownTimer _spawnTimer;
        private Coroutine _fade = null;

        private void Start() {
            foreach (DinoData data in Assets.Instance.DinoData) {
                _lookup.Add(data.Type, data);
            }
            _points = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++) {
                _points[i] = transform.GetChild(i);
            }
            _spawnTimer = new CountDownTimer(_spawnDelay);
            _hightlight = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate() {
            _spawnTimer.Update(Time.fixedDeltaTime);
            if (_spawnTimer.IsFinished && _spawnQueue.Count > 0) {
                Spawn(_spawnQueue.Dequeue());
                _spawnTimer.Reset(_spawnDelay);
            }
        }

        private void Spawn(DinoType type) {
            GameObject spawn = Instantiate(_lookup[type].Prefab, _points[0].position, Helpers.Look2D(_points[0].position, _points[1].position));
            spawn.GetComponent<DinoPathAgent>().Init(_points);
        }

        public void QueueSpawn(DinoType type) {
            _spawnQueue.Enqueue(type);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (_fade != null) {
                StopCoroutine(_fade);
            }
            _fade = _hightlight.FadeAlpha(2.0f, false, this);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (_fade != null) {
                StopCoroutine(_fade);
            }
            _fade = _hightlight.FadeAlpha(2.0f, true, this);
        }
    }
}