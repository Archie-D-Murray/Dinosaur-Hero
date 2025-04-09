using System;
using System.Collections.Generic;

using Entities;
using Entities.Towers;

using ProjectileComponents;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

using Utilities;

public class GridManager : Singleton<GridManager> {
    [SerializeField] private TowerMenu _towerMenu;
    [SerializeField] private TowerData _heldTower = null;
    [SerializeField] private SpriteRenderer _placementIndicator;
    [SerializeField] private GameObject _indicatorPrefab;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private SFXEmitter _emitter;
    [SerializeField] private TileBase[] _illegalTiles;

    private HashSet<Vector3Int> _tilemapOccupied = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, Tower> _towers = new Dictionary<Vector3Int, Tower>();

    public void Start() {
        HashSet<TileBase> illegalTiles = new HashSet<TileBase>(_illegalTiles);
        foreach (Vector3Int tilePosition in _tilemap.cellBounds.allPositionsWithin) {
            if (illegalTiles.Contains(_tilemap.GetTile(tilePosition))) {
                _tilemapOccupied.Add(tilePosition);
            }
        }
        _towerMenu = FindFirstObjectByType<TowerMenu>();
        _placementIndicator = Instantiate(_indicatorPrefab, -Vector3.one, Quaternion.identity).GetComponent<SpriteRenderer>();
        _placementIndicator.gameObject.SetActive(false);
        Debug.Log($"Cell Bounds: {_tilemap.cellBounds.ToString()}, Size: {_tilemap.size}");
        _emitter = GetComponent<SFXEmitter>();
    }

    public void InitializePlacement(TowerData type) {
        _heldTower = type;
        _placementIndicator.gameObject.SetActive(true);
        _placementIndicator.sprite = type.Sprite;
    }

    private void Update() {
        if (_heldTower != null) {
            Vector3Int gridPos = Vector3Int.RoundToInt(Helpers.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition));
            gridPos.z = 0;
            _placementIndicator.transform.position = new Vector3(gridPos.x, gridPos.y, 0f);
            bool validPos = !_towers.ContainsKey(gridPos) && !_tilemapOccupied.Contains(gridPos);
            _placementIndicator.color = validPos ? Color.green : Color.red;
            if (Input.GetMouseButton(0) && validPos && !EventSystem.current.IsPointerOverGameObject()) {
                _emitter.Play(SoundEffectType.TowerPlace);
                _towers[gridPos] = Instantiate(_heldTower.Prefab, _placementIndicator.transform.position, Quaternion.identity).GetComponent<Tower>();
                _placementIndicator.gameObject.SetActive(false);
                Instantiate(Assets.Instance.towerPlaceParticles, _placementIndicator.transform.position, Quaternion.identity)
                    .GetOrAddComponent<AutoDestroy>().Duration = 1f;
                if (!_towerMenu.Place(_heldTower)) {
                    _heldTower = null;
                }
            } else if (Input.GetMouseButtonDown(1)) {
                _placementIndicator.gameObject.SetActive(false);
                _heldTower = null;
            }
        }
    }

    public void RemoveTower(Tower tower) {
        _towers.Remove(Vector3Int.RoundToInt(tower.transform.position));
    }

    public void UpdateCosts() {
        _towerMenu.CheckCosts();
    }
}