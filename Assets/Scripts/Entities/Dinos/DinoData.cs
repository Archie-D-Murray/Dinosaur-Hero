
using UnityEngine;

namespace Entities.Dinos {

    [CreateAssetMenu(menuName = "Dino Data")]
    public class DinoData : ScriptableObject {
        public DinoType Type;
        public GameObject Prefab;
        public int Cost;
        public Sprite Sprite;
        public string Name;
        public string Description;
    }
}