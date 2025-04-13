using Utilities;

using UnityEngine;
using Entities.Dinos;

public class Assets : PersistentSingleton<Assets> {
    public GameObject EnemyDeathParticles;
    public GameObject TowerDeathParticles;
    public GameObject TowerPlaceParticles;
    public GameObject TowerHitParticles;
    public GameObject EnemyHitParticles;

    public DinoData[] DinoData;
}