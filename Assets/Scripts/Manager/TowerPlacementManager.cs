
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Map;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Towers;
using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using Zenject;
namespace Assets.Scripts.Manager
{
    public class TowerPlacementManager : MonoBehaviour
    {
        [Inject]
        private GameDataSO gameData; 

        [Inject]
        MapController mapController;

        [Inject]
        IGameManager gameManager;

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

        public void StartPlacingTower(TowerType towerType, int cost, Action placementCallback)
        {
            if (isPlacing) return;

            placementFinalized = placementCallback;
            currentTowerCost = cost;
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

        private void Start()
        {
            if (gameData.mortarSetCount > 0)
            {
                currentTowerType = TowerType.Mortar;
                foreach (int tower in gameData.filledMortarTowerSlots)
                {
                    PlaceTower(mapController.TowerElements[tower], true);
                }
            }

            if (gameData.turretSetCount > 0)
            {
                currentTowerType = TowerType.Turret;
                foreach (int tower in gameData.filledTurretTowerSlots)
                {
                    PlaceTower(mapController.TowerElements[tower], true);
                }
            }

            if (gameData.mineSetCount > 0)
            {
                currentTowerType = TowerType.Mine;
                foreach (int mine in gameData.activeMineSlots)
                {
                    PlaceTower(mapController.PathElements[mine], true);
                }
            }

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
                UpdateSilhouetteMaterial(isValidPlacement && !hit.transform.parent.GetComponent<TileElement>().isFilled);

                if (Input.GetMouseButtonUp(0))
                {
                    if (isValidPlacement && !hit.transform.parent.GetComponent<TileElement>().isFilled)
                    {
                        PlaceTower(hit.transform.parent.GetComponent<TileElement>());
                        placementFinalized?.Invoke();
                    }
                    else { CancelPlacement(); }
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                CancelPlacement();
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

        private void PlaceTower(TileElement selectedTile, bool placeWithInitializer = false)
        {
            if (selectedTile.isFilled)
                return;

            selectedTile.isFilled = true;
            Vector3 position = selectedTile.transform.position;

            GameObject newTower = container.InstantiatePrefab(towers[currentTowerType].towerPrefab, position, Quaternion.identity, null);
            newTower.transform.rotation = Quaternion.Euler(-90, 0, 0);
            var tower = newTower.GetComponent<Tower>();
            tower.SetSelectedTileElement(selectedTile);
            tower.SetGameData(gameData);

            switch (currentTowerType)
            {
                case TowerType.Turret:
                    (tower as TurretTower).towerSlotIndex = mapController.TowerElements.IndexOf(selectedTile);
                    (tower as TurretTower).towerData = towers[currentTowerType];
                    if (!placeWithInitializer)
                    {
                        gameData.emptyTowerSlots.Remove((tower as TurretTower).towerSlotIndex);
                        gameData.filledTurretTowerSlots.Add((tower as TurretTower).towerSlotIndex);
                        gameData.turretSetCount++;
                    }
                    break;
                case TowerType.Mortar:
                    (tower as MortarTower).towerSlotIndex = mapController.TowerElements.IndexOf(selectedTile);
                    (tower as MortarTower).towerData = towers[currentTowerType];
                    if (!placeWithInitializer)
                    {
                        gameData.emptyTowerSlots.Remove((tower as MortarTower).towerSlotIndex);
                        gameData.filledMortarTowerSlots.Add((tower as MortarTower).towerSlotIndex);
                        gameData.mortarSetCount++;
                    }
                    break;
                case TowerType.Mine:
                    (tower as MineTower).pathIndex = mapController.PathElements.IndexOf(selectedTile);
                    (tower as MineTower).towerData = towers[currentTowerType];
                    if (!placeWithInitializer)
                    {
                        gameData.mineSetCount++;
                        gameData.activeMineSlots.Add((tower as MineTower).pathIndex);
                    }
                    break;
                default:
                    break;
            }
            if (!placeWithInitializer)
            {
                gameData.SpendMoney(currentTowerCost);
                CancelPlacement();
            }
            if (!gameManager.IsGameStarted())
            {
                gameManager.StartGame();
            }
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
