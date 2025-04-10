using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Utilities;

using Entities.Dinos;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DinoMenu : MonoBehaviour {

    [SerializeField] private CanvasGroup _menuGroup;
    [SerializeField] private GameObject _dinoUIPrefab;
    [SerializeField] private DinoData[] _data;
    [SerializeField] private Dictionary<DinoType, Button> _buttons = new Dictionary<DinoType, Button>();
    [SerializeField] private Dictionary<DinoType, DinoData> _lookup = new Dictionary<DinoType, DinoData>();

    public void Start() {
        _menuGroup = GetComponent<CanvasGroup>();
        foreach (DinoData data in _data) {
            Button buy = Instantiate(_dinoUIPrefab, _menuGroup.transform).GetComponent<Button>();
            buy.onClick.AddListener(() => Globals.Instance.Buy(data.Cost, data.Type));
            _buttons.Add(data.Type, buy);
            _lookup.Add(data.Type, data);
        }
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Tab) && _menuGroup.interactable)) {
            Hide();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !_menuGroup.interactable) {
            Show();
            return;
        }
    }

    public void Show() {
        CheckCosts();
        _menuGroup.FadeCanvas(1f, false, this);
    }

    public void CheckCosts() {
        foreach (KeyValuePair<DinoType, Button> kvp in _buttons) {
            kvp.Value.interactable = _lookup[kvp.Key].Cost <= Globals.Instance.Money;
        }
    }

    public void Hide() {
        EventSystem.current.SetSelectedGameObject(null);
        _menuGroup.FadeCanvas(1f, true, this);
    }
}