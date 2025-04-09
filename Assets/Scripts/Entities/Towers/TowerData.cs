using UnityEngine;

namespace Entities.Towers {

    public enum TowerType { Ankylo, Stego, Rex }

    [CreateAssetMenu(menuName = "Tower Data")]
    public class TowerData : ScriptableObject {
        public TowerType Type;
        public GameObject Prefab;
        public int Cost;
        public Sprite Icon;

        public Sprite Sprite { get; internal set; }
    }
}