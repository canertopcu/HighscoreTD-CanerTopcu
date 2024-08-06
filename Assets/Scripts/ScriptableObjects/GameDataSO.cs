using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
    public class GameDataSO : ScriptableObject
    {
        public List<int> filledTurretTowerSlots = new List<int>();
        public List<int> filledMortarTowerSlots = new List<int>();
        public int mineSetCount = 0;
        public int turretSetCount = 0;
        public int mortarSetCount = 0;

        public List<int> activeMineSlots = new List<int>();
        public List<int> emptyTowerSlots = new List<int>();
        public int playerGold;
        public int playerScore;
        public int mainTowerHealth = 100;
        public int mainTowerMaxHP = 100;
        public int gameLevel = 0;
        public float elapsedTime = 0;
        public float incrementTimer = 60;

        public int KillCount = 0;

        public void AddKillCount()
        {
            KillCount++;
        }

        public void HitDamageMainTower(int damage)
        {
            if (mainTowerHealth - damage > 0)
            {
                mainTowerHealth -= damage;
            }
            else
            {
                mainTowerHealth = 0;
            }
        }

        internal void SpendMoney(int cost)
        {
            playerGold -= cost;
        }

        internal void AddMoney(int reward, float multiply)
        {
            playerGold += (int)(reward * Mathf.Pow(multiply, gameLevel));
        }

        public void AddScore(int score, float multiply)
        {
            playerScore += (int)(score * Mathf.Pow(multiply, gameLevel));
        }

        [ContextMenu("ResetElements")]
        public void ResetElements()
        {
            playerGold = 300;
            playerScore = 0;
            filledMortarTowerSlots.Clear();
            filledTurretTowerSlots.Clear();
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
            gameLevel = 0;
            elapsedTime = 0;
            KillCount = 0;
        }
    }
}