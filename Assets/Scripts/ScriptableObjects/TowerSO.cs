using Assets.Scripts.Core.Enums;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTower", menuName = "Towers/New Tower")]
    public class TowerSO : ScriptableObject
    {
        [Header("Basic Info")]
        public string towerName;
        public TowerType towerType;
        public int baseCost;
        public float costMultiplier;

        [Header("Attack Properties")]
        public float attackDamage;
        public float attackMultiplier;
        public float attackRange;
        public float attackDelay;
        public bool isAreaDamage;

        [Header("Setup")]

        [EnumFlags]
        public PlacingType placingType;


        [Header("Visuals")]
        public GameObject towerPrefab;
        public Sprite uiSprite;
        public GameObject explosionPrefab;

        [Header("Special Properties")]
        public float explosionRadius;  
        public float triggerRadius;  
    }
}
