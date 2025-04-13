using System;
using System.Collections.Generic;

using Entities.Dinos;

using UnityEngine;

using Utilities;

[Serializable]
public class DinoStorage {
    public DinoType Type;
    public int Count = 0;

    public DinoStorage(DinoType type) {
        Type = type;
        Count = 0;
    }
}

public class Globals : Singleton<Globals> {
    [SerializeField] private int _money;
    [SerializeField] private DinoStorage[] _storage;

    private Dictionary<DinoType, int> _lookup = new Dictionary<DinoType, int>();

    public int Money => _money;
    public Action<int> OnMoneyChange;
    public DinoStorage Storage(DinoType type) => _storage[_lookup[type]];

    protected override void Awake() {
        base.Awake();
        Array values = Enum.GetValues(typeof(DinoType));
        _storage = new DinoStorage[values.Length];
        for (int i = 0; i < values.Length; i++) {
            _storage[i] = new DinoStorage((DinoType)values.GetValue(i));
            _lookup.Add(_storage[i].Type, i);
        }
        if (TowerLayer == 0) {
            TowerLayer = 1 << LayerMask.NameToLayer("Tower");
        }
        if (DinoLayer == 0) {
            DinoLayer = 1 << LayerMask.NameToLayer("Dino");
        }
    }

    public LayerMask TowerLayer;
    public LayerMask DinoLayer;

    public void ChangeMoney(int change) {
        _money += change;
        OnMoneyChange?.Invoke(_money);
    }

    public void Buy(int cost, DinoType type) {
        ChangeMoney(-cost);
        Storage(type).Count++;
    }
}