﻿using Assets.Scripts.Core.Enums;
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

        private GameDataSO _gameData;
        private GameDataSO gameData {
            get { 
                if(_gameData == null)
                {
                    _gameData = Resources.Load<GameDataSO>("Data/GameData");
                }
                return _gameData;
            }
        }

        [Inject]
        private TowerPlacementManager towerPlacementManager;

        [SerializeField] private TextMeshProUGUI priceText;
        public GameObject blockImage;

        private void Start()
        { 
            eventTrigger= GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            entry.callback.AddListener((data) => { OnPointerDown(); });
            eventTrigger.triggers.Add(entry);

             
            priceText.text = cost.ToString();
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
            Debug.Log(towerType+" Clicked");
            towerPlacementManager.StartPlacingTower(towerType,cost);
        }

        public void SetCost(int cost)
        {
            this.cost = cost;
            priceText.text = cost.ToString();
        }
    }
}
