using UnityEngine;
using Utilities;
using System;
using Entities.Dinos;
using System.Linq;
using System.Collections.Generic;
using Unity.Collections;

public class GameManager : Singleton<GameManager> {
    public Action OnLose;
    public Action OnWin;

    private int _initialMoney;
    private DinoStorage[] _storage;

    [SerializeField] private List<Dino> _dinos = new List<Dino>();

    private void Start() {
        CopyCurrent();
    }

    public void RegisterDino(Dino dino) {
        _dinos.Add(dino);
    }

    public void UnregisterDino(Dino dino) {
        _dinos.RemoveSwapBack(dino);
        if (_dinos.Count == 0 && Globals.Instance.Empty()) {
            OnLose?.Invoke();
        }
    }

    private void CopyCurrent() {
        _initialMoney = Globals.Instance.Money;
        DinoType[] _types = Enum.GetValues(typeof(DinoType)).Cast<DinoType>().ToArray();
        _storage = new DinoStorage[Globals.Instance.StorageCount];
        for (int i = 0; i < _storage.Length; i++) {
            _storage[i] = new DinoStorage(_types[i], Globals.Instance.Storage(_types[i]).Count);
        }
    }

    public void Reset() {
        Globals.Instance.Reset(_initialMoney, _storage);
    }
}