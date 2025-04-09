using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Utilities;

using Entities.Towers;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TowerMenu : MonoBehaviour {

    [SerializeField] private GameObject[] towers;
    [SerializeField] private CanvasGroup menuGroup;
    [SerializeField] private GameObject towerUIPrefab;
    [SerializeField] private TowerData[] _data;
    [SerializeField] private Dictionary<TowerType, Button> _buttons = new Dictionary<TowerType, Button>();
    [SerializeField] private Dictionary<TowerType, TowerData> _lookup = new Dictionary<TowerType, TowerData>();

    public void Start() {
        menuGroup = GetComponent<CanvasGroup>();
        foreach (TowerData data in _data) {
            Button buy = Instantiate(towerUIPrefab, menuGroup.transform).GetComponent<Button>();
            buy.onClick.AddListener(() => StartPlacingTower(data));
            _buttons.Add(data.Type, buy);
            _lookup.Add(data.Type, data);
        }
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Tab) && menuGroup.interactable)) {
            Hide();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !menuGroup.interactable) {
            Show();
            return;
        }
    }

    public bool Place(TowerData tower) {
        Globals.Instance.ChangeMoney(-tower.Cost);
        CheckCosts();
        if (Globals.Instance.Money >= tower.Cost) {
            StartPlacingTower(tower);
            return true;
        }
        return false;
    }

    public void Show() {
        CheckCosts();
        menuGroup.FadeCanvas(1f, false, this);
    }

    public void CheckCosts() {
        foreach (KeyValuePair<TowerType, Button> kvp in _buttons) {
            kvp.Value.interactable = _lookup[kvp.Key].Cost <= Globals.Instance.Money;
        }
    }

    public void Hide() {
        EventSystem.current.SetSelectedGameObject(null);
        menuGroup.FadeCanvas(1f, true, this);
    }

    private void StartPlacingTower(TowerData data) {
        if (Globals.Instance.Money < data.Cost) {
            return;
        }
        Vector3 mousePos = Helpers.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        GridManager.Instance.InitializePlacement(data);
    }
}