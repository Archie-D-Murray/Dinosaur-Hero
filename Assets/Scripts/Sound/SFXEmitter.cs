using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Utilities;

public enum SoundEffectType { None, Shoot, Hit, Death, TowerPlace, UIClick }

public class SFXEmitter : MonoBehaviour {
    [SerializeField] private Dictionary<SoundEffectType, AudioSource> _sources;
    [SerializeField] private SoundEffect[] _effects;
    const float PITCH_BEND_AMOUNT = 10f;

    private void Start() {
        _sources = new Dictionary<SoundEffectType, AudioSource>();
        foreach (SoundEffect soundEffect in _effects) {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            _sources.Add(soundEffect.Type, audioSource);
            audioSource.outputAudioMixerGroup = SoundManager.Instance.SFX;
            audioSource.clip = soundEffect.Clip;
        }
    }

    public void Play(SoundEffectType soundEffect) {
        if (soundEffect == SoundEffectType.None) {
            return;
        }
        if (_sources.ContainsKey(soundEffect)) {
            _sources[soundEffect].Play();
        }
    }

    public void Play(SoundEffectType soundEffect, float pitchRandomisation) {
        if (_sources.ContainsKey(soundEffect)) {
            _sources[soundEffect].pitch += pitchRandomisation / PITCH_BEND_AMOUNT;
            _sources[soundEffect].Play();
            StartCoroutine(ResetClip(soundEffect));
        }
    }

    private IEnumerator ResetClip(SoundEffectType soundEffect) {
        while (_sources[soundEffect].isPlaying) {
            yield return Yielders.WaitForSeconds(0.1f);
        }
        _sources[soundEffect].pitch = 1f;
    }

    public float Length(SoundEffectType soundEffect) {
        if (_sources.TryGetValue(soundEffect, out AudioSource source)) {
            return source.clip.length;
        }
        return 0f;
    }
}