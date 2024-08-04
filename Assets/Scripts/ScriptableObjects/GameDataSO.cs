using Assets.Scripts.Core.Enums;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{

    [CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
    public class GameDataSO : ScriptableObject
    {
        public SerializedDictionary<int, TowerType> filledTowerSlots;
        public int mineSetCount = 0;
        public int turretSetCount = 0;
        public int mortarSetCount = 0;

        public List<int> activeMineSlots;
        public List<int> emptyTowerSlots;
        public int playerGold;
        public int playerScore;
        public int mainTowerHealth = 100;
        public int mainTowerMaxHP = 100;

        public void HitDamageMainTower(int damage)
        {
            if(mainTowerHealth-damage>0){
                mainTowerHealth -= damage;
            }
            else{
                mainTowerHealth = 0;
            } 
        }

        internal void SpendMoney(int cost)
        {
            playerGold -= cost;
        }

        [ContextMenu("ResetElements")]
        public void ResetElements()
        {
            playerGold = 1000;
            playerScore = 0;
            filledTowerSlots.Clear();
            emptyTowerSlots.Clear();
            for (int i = 0; i < 12; i++)
            {
                emptyTowerSlots.Add(i);
            }
            mineSetCount = 0;
            turretSetCount = 0;
            mortarSetCount = 0;
            activeMineSlots.Clear();
            mainTowerHealth = mainTowerMaxHP;
        }
    }
}
