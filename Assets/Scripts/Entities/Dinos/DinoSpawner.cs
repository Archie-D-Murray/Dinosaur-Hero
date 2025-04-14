using System;
using System.Collections.Generic;

using UI;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        [SerializeField] private GameObject _queueUIPrefab;
        [SerializeField] private GameObject _queueObjectPrefab;

        private Dictionary<DinoType, DinoData> _lookup = new Dictionary<DinoType, DinoData>();
        private Queue<DinoType> _spawnQueue = new Queue<DinoType>();
        private CountDownTimer _spawnTimer;
        private Transform _queueUI;
        private CanvasFader _fader;
        [SerializeField] private Image _progress;

        private void Start() {
            foreach (DinoData data in Assets.Instance.DinoData) {
                _lookup.Add(data.Type, data);
            }
            _points = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++) {
                _points[i] = transform.GetChild(i);
            }
            _spawnTimer = new CountDownTimer(_spawnDelay);
            _queueUI = Instantiate(_queueUIPrefab, UIManager.Instance.WorldCanvas).transform;
            _queueUI.transform.position = transform.position + Vector3.up;
            _fader = _queueUI.GetComponent<CanvasFader>();
            _queueUI = _queueUI.GetChild(0).GetChild(0);
        }

        private void FixedUpdate() {
            if (_spawnQueue.Count > 0) {
                _spawnTimer.Update(Time.fixedDeltaTime);
            }
            if (_progress) {
                _progress.fillAmount = _spawnTimer.Progress();
            } else {
                if (_queueUI.childCount > 0) {
                    _progress = _queueUI.GetChild(0).GetComponent<Image>();
                }
            }
            if (_spawnTimer.IsFinished && _spawnQueue.Count > 0) {
                Spawn(_spawnQueue.Dequeue());
                Destroy(_queueUI.GetChild(0).gameObject);
                _spawnTimer.Reset(_spawnDelay);
            }
        }

        private void Spawn(DinoType type) {
            GameObject spawn = Instantiate(_lookup[type].Prefab, _points[0].position, Helpers.Look2D(_points[0].position, _points[1].position));
            spawn.GetComponent<DinoPathAgent>().Init(_points);
        }

        public void QueueSpawn(DinoType type) {
            _spawnQueue.Enqueue(type);
            Instantiate(_queueObjectPrefab, _queueUI).GetComponent<Image>().sprite = _lookup[type].Sprite;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _fader.Show();
        }

        public void OnPointerExit(PointerEventData eventData) {
            _fader.Hide();
        }
    }
}