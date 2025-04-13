using System;

using UnityEngine;
using UnityEngine.UI;

using Utilities;

namespace Entities.Towers {
    public class TowerRespawn : MonoBehaviour {
        [SerializeField] private TowerType _type;
        [SerializeField] private CountDownTimer _respawnTimer;
        [SerializeField] private Image _image;

        private bool _init = false;

        private void Start() {
        }

        public void Init(TowerType type, float respawnTime) {
            _type = type;
            _respawnTimer = new CountDownTimer(respawnTime);
            _respawnTimer.Start();
            _init = true;
        }

        private void FixedUpdate() {
            if (!_init) { return; }
            _respawnTimer.Update(Time.fixedDeltaTime);
            _image.fillAmount = _respawnTimer.Progress();
            if (_respawnTimer.IsFinished) {
                TowerManager.Instance.SpawnTower(_type, transform.position);
                Destroy(gameObject);
            }
        }
    }
}