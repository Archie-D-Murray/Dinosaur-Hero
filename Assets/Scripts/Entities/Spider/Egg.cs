using UnityEngine;

namespace Spider {
    public class Egg : MonoBehaviour {
        [SerializeField] private float spawnDelay;
        [SerializeField] private GameObject babySpiderPrefab;
        [SerializeField] private Transform[] points;
        [SerializeField] private int pointIndex;

        [SerializeField] private float timer = 0f;

        public void Init(Transform[] points, int pointIndex) {
            this.points = points;
            this.pointIndex = pointIndex;
        }

        public void FixedUpdate() {
            timer += Time.fixedDeltaTime;
            if (timer >= spawnDelay) {
                GameObject spider = Instantiate(babySpiderPrefab, transform.position, transform.rotation);
                spider.GetComponent<BabySpider>().Init(points, pointIndex);
                Destroy(gameObject);
            }
        }
    }
}