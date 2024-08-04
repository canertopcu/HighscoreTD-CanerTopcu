namespace Assets.Scripts.Manager
{
    using Assets.Scripts.Core.Enums;
    using Assets.Scripts.ScriptableObjects;
    using AYellowpaper.SerializedCollections;
    using UnityEngine;
    using Zenject;

    public class TowerPlacementManager : MonoBehaviour
    {
        [Inject]
        private GameDataSO gameData;

        public SerializedDictionary<TowerType, TowerSO> towers;
        public Material validPlacementMaterial;
        public Material invalidPlacementMaterial;

        private GameObject currentTowerSilhouette;
        private TowerType currentTowerType;
        private bool isPlacing = false;

        [Inject]
        private DiContainer container;


        public void StartPlacingTower(TowerType towerType)
        {
            if (isPlacing) return;

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
                currentTowerSilhouette.transform.position = hit.point;

                bool isValidPlacement = IsValidPlacement(hit.point);
                UpdateSilhouetteMaterial(isValidPlacement);

                if (Input.GetMouseButtonDown(0) && isValidPlacement)
                {
                    PlaceTower(hit.point);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }

        private bool IsValidPlacement(Vector3 position)
        {
            return true;
        }

        private void UpdateSilhouetteMaterial(bool isValid)
        {
            Renderer[] renderers = currentTowerSilhouette.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = isValid ? validPlacementMaterial : invalidPlacementMaterial;
            }
        }

        private void PlaceTower(Vector3 position)
        {
            GameObject newTower = container.InstantiatePrefab(towers[currentTowerType].towerPrefab, position, Quaternion.identity, null);
            gameData.SpendMoney(newTower.GetComponent<Tower>().cost);
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
