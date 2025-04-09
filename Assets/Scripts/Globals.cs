using UnityEngine;

using Utilities;

public class Globals : Singleton<Globals> {
    [SerializeField] private int _money;
    public int Money => _money;

    public void ChangeMoney(int change) {
        _money += change;
        GridManager.Instance.UpdateCosts();
    }
}