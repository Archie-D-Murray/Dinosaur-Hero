using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Utilities;

using Entities.Dinos;

[Serializable]
public class DinoGroup {
    public DinoType Type;
    public int Count = 1;
}

public class DinoSpawner : MonoBehaviour {
    [SerializeField] private Transform[] _points;
    [SerializeField] private DinoData[] _data;
    [SerializeField] float _spawnDelay = 1.0f;

    private Dictionary<DinoType, DinoData> _lookup = new Dictionary<DinoType, DinoData>();
    private Queue<DinoType> _spawnQueue = new Queue<DinoType>();
    private CountDownTimer _spawnTimer;

    private void Start() {
        foreach (DinoData data in _data) {
            _lookup.Add(data.Type, data);
        }
        _points = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            _points[i] = transform.GetChild(i);
        }
        _spawnTimer = new CountDownTimer(_spawnDelay);
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
        spawn.transform.parent = transform;
    }

    public void QueueSpawn(DinoType type) {
        _spawnQueue.Enqueue(type);
    }
}