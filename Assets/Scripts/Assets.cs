using Utilities;

using UnityEngine;
using Entities.Dinos;
using Entities.Towers;
using System.Collections.Generic;
using System;
using Entities;

public class Assets : Singleton<Assets> {

    protected override void Awake() {
        base.Awake();
        foreach (TowerParticles particle in TowerParticlesData) {
            _towerParticles.Add(particle.Type, particle.Particles);
        }
        foreach (EffectParticles particle in EffectParticlesData) {
            _effectParticles.Add(particle.Type, particle.Particles);
        }
    }

    [Serializable]
    struct TowerParticles {
        public TowerType Type;
        public GameObject Particles;
    }

    [Serializable]
    struct EffectParticles {
        public EffectType Type;
        public GameObject Particles;
    }

    public GameObject DinoHitParticles;

    private Dictionary<TowerType, GameObject> _towerParticles = new Dictionary<TowerType, GameObject>();
    private Dictionary<EffectType, GameObject> _effectParticles = new Dictionary<EffectType, GameObject>();

    [SerializeField] private TowerParticles[] TowerParticlesData;
    [SerializeField] private EffectParticles[] EffectParticlesData;
    public Material DamageFlash;
    public DinoData[] DinoData;
    public TowerData[] TowerData;

    public int EffectParticleCount => EffectParticlesData.Length;

    public GameObject GetTowerParticles(TowerType type) {
        return _towerParticles[type];
    }

    public GameObject GetEffectParticles(EffectType type) {
        return _effectParticles[type];
    }
}