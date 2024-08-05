using Assets.Scripts.Core.Enums;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{ 
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/New Enemy")]
    public class EnemySO : ScriptableObject
    {
        [Header("Basic Info")]
        public string enemyName;
        public EnemyType enemyTypeType;
        public int Hp;
        public float hpMultiplier;
        public int goldReward;
        public float goldRewardMultiplier;
        public int scoreReward;
        public float scoreRewardMultiplier;

        [Header("Attack Properties")]
        public int attackDamage; 

        [Header("Visuals")]
        public GameObject enemyPrefab;
    }
}
