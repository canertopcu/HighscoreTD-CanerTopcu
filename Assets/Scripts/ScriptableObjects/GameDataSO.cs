using Assets.Scripts.Core.Enums;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
   
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
    public class GameDataSO : ScriptableObject
    {
        public SerializedDictionary<int, TowerType> filledTowerSlots;
        public List<int> emptyTowerSlots;
        public int playerGold;
        public int playerScore;

        internal void SpendMoney(int cost)
        {
            playerGold -= cost;
        }
    }
}
