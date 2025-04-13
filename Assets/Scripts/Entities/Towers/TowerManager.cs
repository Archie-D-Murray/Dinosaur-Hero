using System.Collections.Generic;

using Unity.Collections;

using UnityEngine;

using Utilities;

using UI;

namespace Entities.Towers {
    public class TowerManager : Singleton<TowerManager> {
        [SerializeField] private GameObject _respawnPrefab;
        [SerializeField] private List<Tower> _towers = new List<Tower>();
        [SerializeField] private float _respawnTime = 5.0f;

        private Dictionary<TowerType, GameObject> _towerPrefabs = new Dictionary<TowerType, GameObject>();

        private void Start() {
            foreach (TowerData data in Assets.Instance.TowerData) {
                _towerPrefabs.Add(data.Type, data.Prefab);
            }
        }

        public void OnTowerDestroy(Tower tower) {
            GameObject respawn = Instantiate(_respawnPrefab, UIManager.Instance.WorldCanvas);
            respawn.transform.SetPositionAndRotation(tower.transform.position, Quaternion.identity);
            respawn.GetComponent<TowerRespawn>().Init(tower.Type, _respawnTime);
            _towers.RemoveSwapBack(tower);
        }

        public void SpawnTower(TowerType type, Vector3 position) {
            GameObject tower = Instantiate(_towerPrefabs[type], transform);
            tower.transform.SetPositionAndRotation(position, Quaternion.identity);
        }

        public void RegisterTower(Tower tower) {
            _towers.Add(tower);
        }

        public void DisableAllTowers() {
            foreach (Tower tower in _towers) {
                tower.DisableTower();
            }
        }
    }
}