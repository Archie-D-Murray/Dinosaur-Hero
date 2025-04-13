using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Utilities;

using Entities.Dinos;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using Tags.UI;

public class DinoMenu : MonoBehaviour {

    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private GameObject _dinoBuyPrefab;
    [SerializeField] private GameObject _dinoStoragePrefab;
    [SerializeField] private Dictionary<DinoType, Button> _buttons = new Dictionary<DinoType, Button>();
    [SerializeField] private Dictionary<DinoType, TMP_Text> _storage = new Dictionary<DinoType, TMP_Text>();
    [SerializeField] private Dictionary<DinoType, DinoData> _lookup = new Dictionary<DinoType, DinoData>();
    [SerializeField] private Transform _storageRoot;
    [SerializeField] private Transform _buyRoot;

    public void Start() {
        _canvas = GetComponent<CanvasGroup>();
        foreach (DinoData data in Assets.Instance.DinoData) {
            GameObject buySlot = Instantiate(_dinoBuyPrefab, _buyRoot);
            Button buy = buySlot.GetComponentInChildren<Button>();
            buySlot.GetComponentsInChildren<Image>().First(image => image.gameObject.HasComponent<IconTag>()).sprite = data.Sprite;
            foreach (TMP_Text text in buySlot.GetComponentsInChildren<TMP_Text>()) {
                if (text.gameObject.HasComponent<ReadoutTag>()) {
                    text.text = data.Name;
                }
                if (text.gameObject.HasComponent<PriceTag>()) {
                    text.text = data.Cost.ToString();
                }
            }
            buy.onClick.AddListener(() => Globals.Instance.Buy(data.Cost, data.Type));
            _buttons.Add(data.Type, buy);
            _lookup.Add(data.Type, data);

            GameObject storageSlot = Instantiate(_dinoStoragePrefab, _storageRoot);
            storageSlot.GetComponentsInChildren<Image>().First(image => image.gameObject.HasComponent<IconTag>()).sprite = data.Icon;
            TMP_Text counter = storageSlot.GetComponentInChildren<TMP_Text>();
            counter.text = $"x{Globals.Instance.Storage(data.Type).Count}";
            _storage.Add(data.Type, counter);
        }
        CheckCosts();
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Tab) && _canvas.interactable)) {
            Hide();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !_canvas.interactable) {
            CheckCosts();
            Show();
            return;
        }
    }

    public void Show() {
        CheckCosts();
        _canvas.FadeCanvas(1f, false, this);
    }

    public void CheckCosts() {
        foreach (KeyValuePair<DinoType, Button> kvp in _buttons) {
            kvp.Value.interactable = _lookup[kvp.Key].Cost <= Globals.Instance.Money;
        }
        foreach (KeyValuePair<DinoType, TMP_Text> kvp in _storage) {
            kvp.Value.text = $"x{Globals.Instance.Storage(kvp.Key).Count}";
        }
    }

    public void Hide() {
        EventSystem.current.SetSelectedGameObject(null);
        _canvas.FadeCanvas(1f, true, this);
    }
}