using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

[RequireComponent(typeof(SFXEmitter))]
public class ButtonSFX : MonoBehaviour, IPointerEnterHandler {
    [SerializeField] private SFXEmitter _emitter;
    [SerializeField] private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => _emitter.Play(SoundEffectType.UIClick));
        _emitter = GetComponent<SFXEmitter>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _emitter.Play(SoundEffectType.UIHover);
    }
}