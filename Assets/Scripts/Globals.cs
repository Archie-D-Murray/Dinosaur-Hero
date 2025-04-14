using System;
using System.Collections.Generic;
using System.Linq;

using Entities.Dinos;

using UnityEngine;

using Utilities;

[Serializable]
public class DinoStorage {
    public DinoType Type;
    public int Count = 0;

    public DinoStorage(DinoType type, int count = 0) {
        Type = type;
        Count = count;
    }
}

public class Globals : PersistentSingleton<Globals> {
    [SerializeField] private int _money;
    [SerializeField] private DinoStorage[] _storage;

    private Dictionary<DinoType, int> _lookup = new Dictionary<DinoType, int>();

    public int Money => _money;
    public int StorageCount => _storage.Length;
    public Action<int> OnMoneyChange;
    public DinoStorage Storage(DinoType type) => _storage[_lookup[type]];

    protected override void Awake() {
        base.Awake();
        InitStorage();
        if (TowerLayer == 0) {
            TowerLayer = 1 << LayerMask.NameToLayer("Tower");
        }
        if (DinoLayer == 0) {
            DinoLayer = 1 << LayerMask.NameToLayer("Dino");
        }
    }

    public LayerMask TowerLayer;
    public LayerMask DinoLayer;
    [SerializeField] private int _startingMoney = 500;

    public void ChangeMoney(int change) {
        _money += change;
        OnMoneyChange?.Invoke(_money);
    }

    public bool Empty() {
        return _storage.Sum(storage => storage.Count) == 0;
    }

    public void Buy(int cost, DinoType type) {
        ChangeMoney(-cost);
        Storage(type).Count++;
    }

    public void Reset(int initialMoney, DinoStorage[] storage) {
        _money = initialMoney;
        for (int i = 0; i < storage.Length; i++) {
            _storage[i] = new DinoStorage(storage[i].Type, storage[i].Count);
        }
    }

    public void InitStorage() {
        _lookup.Clear();
        DinoType[] values = Enum.GetValues(typeof(DinoType)).Cast<DinoType>().ToArray();
        _storage = new DinoStorage[values.Length];
        _money = _startingMoney;
        for (int i = 0; i < values.Length; i++) {
            _storage[i] = new DinoStorage(values[i]);
            _lookup.Add(_storage[i].Type, i);
        }
    }

    public void RetriveLiveDinos(IEnumerable<Dino> dinos) {
        foreach (Dino dino in dinos) {
            Globals.Instance.Storage(dino.Type).Count++;
        }
    }
}