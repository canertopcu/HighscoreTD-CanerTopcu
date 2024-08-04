using Assets.Scripts.Core.Enums;
using Assets.Scripts.Manager;
using Assets.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts
{

    public class TowerButton : MonoBehaviour
    {
        public TowerType towerType;
        public int cost;
        //private Button button;
        private EventTrigger eventTrigger;

        private GameDataSO gameData;

        [Inject]
        private TowerPlacementManager towerPlacementManager;

        [SerializeField] private TextMeshProUGUI priceText;
        public GameObject blockImage;

        private void Start()
        {
            gameData = Resources.Load<GameDataSO>("Data/GameData");
            eventTrigger= GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            entry.callback.AddListener((data) => { OnClick(); });
            eventTrigger.triggers.Add(entry);

             
            priceText.text = cost.ToString();
        }

        private void Update()
        {
            blockImage.SetActive(gameData.playerGold < cost);
            eventTrigger.enabled = gameData.playerGold >= cost; 
        }

        private void OnClick()
        {
            Debug.Log(towerType+" Clicked");
            towerPlacementManager.StartPlacingTower(towerType);
        }
    }
}
