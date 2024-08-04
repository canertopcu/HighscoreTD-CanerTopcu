﻿namespace Assets.Scripts.Manager
{
    using Assets.Scripts.Core.Enums;
    using Assets.Scripts.ScriptableObjects;
    using AYellowpaper.SerializedCollections;
    using System;
    using UnityEngine;
    using Zenject;

    public class TowerPlacementManager : MonoBehaviour
    {
        [Inject]
        private GameDataSO gameData;

        [Inject]
        private VfxDataSO vfxData;

        [Inject]
        MapController mapController;



        public SerializedDictionary<TowerType, TowerSO> towers;
        public Material validPlacementMaterial;
        public Material invalidPlacementMaterial;

        private GameObject currentTowerSilhouette;
        private TowerType currentTowerType;
        private int currentTowerCost;
        [SerializeField] private bool isPlacing = false;

        [Inject]
        private DiContainer container;
        private Action placementFinalized;

        public void StartPlacingTower(TowerType towerType,int cost,Action placementCallback)
        {
            if (isPlacing) return;

            placementFinalized = placementCallback;
            currentTowerCost= cost;
            currentTowerType = towerType;
            GameObject towerPrefab = towers[towerType].towerPrefab;
            currentTowerSilhouette = container.InstantiatePrefab(towerPrefab);

            Renderer[] renderers = currentTowerSilhouette.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = validPlacementMaterial;
            }

            isPlacing = true;
        }

        private void Update()
        {
            if (!isPlacing) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                bool isValidPlacement = false;
                TileElement selectedElement = null;
                if (hit.collider.transform.parent != null)
                {
                    hit.collider.transform.parent.TryGetComponent(out selectedElement);
                    if (selectedElement != null)
                    {
                        if ((towers[currentTowerType].placingType & PlacingType.OnPath) == PlacingType.OnPath)
                        {
                            if (selectedElement.TileType == TileType.Path)
                            {
                                isValidPlacement = true;
                            }
                        }
                        else if ((towers[currentTowerType].placingType & PlacingType.OnDock) == PlacingType.OnDock)
                        {
                            if (selectedElement.TileType == TileType.TowerPlacement)
                            {
                                isValidPlacement = true;
                            }
                        }
                        else if ((towers[currentTowerType].placingType & (PlacingType.OnPath | PlacingType.OnDock)) == (PlacingType.OnPath | PlacingType.OnDock))
                        {
                            if (selectedElement.TileType == TileType.Path ||
                                selectedElement.TileType == TileType.TowerPlacement)
                            {
                                isValidPlacement = true;
                            }
                        }
                    }
                }
                currentTowerSilhouette.transform.position = hit.point;
                UpdateSilhouetteMaterial(isValidPlacement);

                if (Input.GetMouseButtonUp(0))
                {
                    if (isValidPlacement)
                    {
                        PlaceTower(hit.transform.parent.GetComponent<TileElement>());
                        placementFinalized?.Invoke();
                    }
                    else { CancelPlacement(); }
                    Debug.Log("Mouse Up");
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                CancelPlacement();
                Debug.Log("Mouse Up2");

            }

        }

        private void UpdateSilhouetteMaterial(bool isValid)
        {
            Renderer[] renderers = currentTowerSilhouette.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = isValid ? validPlacementMaterial : invalidPlacementMaterial;
            }
        }

        private void PlaceTower(TileElement selectedTile)
        {
            Vector3 position = selectedTile.transform.position;
           
            GameObject newTower = container.InstantiatePrefab(towers[currentTowerType].towerPrefab, position, Quaternion.identity, null);
            newTower.transform.rotation = Quaternion.Euler(-90,0, 0);
            var tower = newTower.GetComponent<Tower>(); 

            tower.SetGameData(gameData);

            switch (currentTowerType)
            {
                case TowerType.Turret:
                    (tower as TurretTower).towerSlotIndex= mapController.TowerElements.IndexOf(selectedTile);
                    gameData.emptyTowerSlots.Remove((tower as TurretTower).towerSlotIndex);
                    gameData.filledTowerSlots.Add((tower as TurretTower).towerSlotIndex,currentTowerType);
                    gameData.turretSetCount++;
                    break;
                case TowerType.Mortar:
                    (tower as MortarTower).towerSlotIndex = mapController.TowerElements.IndexOf(selectedTile);
                    gameData.emptyTowerSlots.Remove((tower as MortarTower).towerSlotIndex);
                    gameData.filledTowerSlots.Add((tower as MortarTower).towerSlotIndex, currentTowerType);
                    gameData.mortarSetCount++;
                    break;
                case TowerType.Mine:
                    (tower as MineTower).pathIndex = mapController.PathElements.IndexOf(selectedTile);
                    tower.SetVfxData(vfxData);
                    gameData.mineSetCount++;  
                    gameData.activeMineSlots.Add((tower as MineTower).pathIndex);
                    break;
                default:
                    break;
            }

            gameData.SpendMoney(currentTowerCost);
            CancelPlacement();
        }

        private void CancelPlacement()
        {
            if (currentTowerSilhouette != null)
            {
                Destroy(currentTowerSilhouette);
            }
            isPlacing = false;
        }
    }
}
