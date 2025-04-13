using UnityEngine;

namespace Entities.Towers {
    [CreateAssetMenu(menuName = "Tower Data")]
    public class TowerData : ScriptableObject {
        public GameObject Prefab;
        public TowerType Type;
    }
}