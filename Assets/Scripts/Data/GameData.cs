using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    public class GameData
    {
        public List<int> filledTurretTowerSlots;
        public List<int> filledMortarTowerSlots;
        public int mineSetCount;
        public int turretSetCount;
        public int mortarSetCount;
        public List<int> activeMineSlots;
        public List<int> emptyTowerSlots;
        public int playerGold;
        public int playerScore;
        public int mainTowerHealth;
        public int mainTowerMaxHP;
        public int gameLevel;
        public float elapsedTime;
        public float incrementTimer;
        public int KillCount;
    }
}
