using Assets.Scripts.Core.Enums;
using Assets.Scripts.Manager;
using Assets.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Scripts
{

    public class TowerButton : MonoBehaviour
    {
        public TowerType towerType;
        public int cost;
        //private Button button;
        private EventTrigger eventTrigger;

        [Inject]
        private GameDataSO gameData;

        [Inject]
        private TowerPlacementManager towerPlacementManager;

        [SerializeField] private TextMeshProUGUI priceText;
        public GameObject blockImage;

        private void Start()
        {
            eventTrigger = GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            entry.callback.AddListener((data) => { OnPointerDown(); });
            eventTrigger.triggers.Add(entry);

           
            UpdateCost();
            
        }


        private void OnDisable()
        {
            eventTrigger.triggers.Clear();
        }

        private void Update()
        {
            blockImage.SetActive(gameData.playerGold < cost);
            eventTrigger.enabled = gameData.playerGold >= cost;
        }

        private void OnPointerDown()
        {
            Debug.Log(towerType + " Clicked");
            towerPlacementManager.StartPlacingTower(towerType, cost,UpdateCost);
        }

        public void UpdateCost()
        {
            TowerSO tower = towerPlacementManager.towers[towerType];
            switch (towerType)
            {
                case TowerType.Turret:
                    cost = (int)(tower.baseCost * Mathf.Pow(tower.costMultiplier, (float)gameData.turretSetCount));
                    break;
                case TowerType.Mortar:
                    cost = (int)(tower.baseCost * Mathf.Pow(tower.costMultiplier, (float)gameData.mortarSetCount));
                    break;
                case TowerType.Mine:
                    cost = (int)(tower.baseCost * Mathf.Pow(tower.costMultiplier, (float)gameData.mineSetCount));
                    break;

            }
            priceText.text = cost.ToString();
        }
    }
}
