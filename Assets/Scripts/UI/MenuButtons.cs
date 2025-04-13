using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class MenuButtons : MonoBehaviour {
        [SerializeField] private KeyCode _storage = KeyCode.F1;
        [SerializeField] private KeyCode _spawner = KeyCode.F2;

        [SerializeField] private StorageUI _storageUI;
        [SerializeField] private SpawnerUI _spawnerUI;

        [SerializeField] private Button _storageButton;
        [SerializeField] private Button _spawnerButton;

        private void Start() {
            if (!_storageUI) {
                _storageUI = FindFirstObjectByType<StorageUI>();
            }
            if (!_spawnerUI) {
                _spawnerUI = FindFirstObjectByType<SpawnerUI>();
            }

            _spawnerButton.onClick.AddListener(ToggleSpawner);
            _storageButton.onClick.AddListener(ToggleStorage);
        }

        private void Update() {
            if (Input.GetKeyDown(_storage)) {
                ToggleStorage();
            }
            if (Input.GetKeyDown(_spawner)) {
                ToggleSpawner();
            }
        }

        private void ToggleSpawner() {
            _spawnerUI.Toggle();
            _storageUI.Hide();
        }

        private void ToggleStorage() {
            _storageUI.Toggle();
            _spawnerUI.Hide();
        }
    }
}