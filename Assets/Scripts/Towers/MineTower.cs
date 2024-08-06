using Assets.Scripts.Enemy;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace Assets.Scripts.Towers
{
    public class MineTower : Tower
    {
        public bool isExploded = false;
        public int pathIndex = -1;
        public TowerSO towerData;
        public List<EnemyController> enemies = new List<EnemyController>();

        private GameDataSO _gameData; 
        [Inject]
        private void Construct(GameDataSO gameData)
        {
            _gameData = gameData;
        }

        public override void Setlment()
        {
            base.Setlment();
            transform.GetComponent<Collider>().enabled = true; 
        }

        private void Update()
        {
            Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.up, out RaycastHit hit);
            if (hit.collider != null && !isExploded)
            {
                OnExplode();
            }
        }

        [ContextMenu("Explode")]
        public void OnExplode()
        {
            if (pathIndex != -1)
            {
                isExploded = true;
                if (_gameData.activeMineSlots.Contains(pathIndex))
                { 
                    _gameData.activeMineSlots.Remove(pathIndex);
                }
               
                var vfx = Instantiate(towerData.explosionPrefab, transform.position, Quaternion.identity);
                GetComponent<MeshRenderer>().enabled = false;
                if (enemies.Count > 0)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        enemies[i].TakeDamage(towerData.attackDamage*Mathf.Pow(towerData.attackMultiplier, _gameData.gameLevel)); 
                    }
                }
                selectedTileElement.isFilled = false;
                Destroy(vfx, 2f);
                Destroy(gameObject, 3f);

            }
        }
        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out EnemyController enemy);
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            enemies.Remove(other.GetComponent<EnemyController>());

        }
    }

}
