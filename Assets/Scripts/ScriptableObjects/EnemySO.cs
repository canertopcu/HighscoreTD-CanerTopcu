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
        public float Hp;
        public float hpMultiplier;

        [Header("Attack Properties")]
        public float attackDamage; 

        [Header("Visuals")]
        public GameObject enemyPrefab;
    }
}
