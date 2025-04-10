
using UnityEngine;

namespace Entities.Dinos {

    [CreateAssetMenu(menuName = "Dino Data")]
    public class DinoData : ScriptableObject {
        public DinoType Type;
        public GameObject Prefab;
        public int Cost;
        public Sprite Icon;

        public Sprite Sprite { get; internal set; }
    }
}