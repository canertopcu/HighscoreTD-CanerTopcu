using Assets.Scripts.Enemy;
using Assets.Scripts.ScriptableObjects;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ammo
{
    public class Missile : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        IMemoryPool _pool;
        public float duration = 1f;
        public float arcHeight = 5f;
        private GameObject _explosion;
        private TowerSO _towerData;
        private GameDataSO _gameData;
        private float _damageWhenHit;
        public List<EnemyController> enemies = new List<EnemyController>();
        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }

        public void OnDespawned()
        {
            // Cleanup logic here
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        [ContextMenu("Move Projectile")]
        public void MoveProjectile(Vector3 targetPos, float damageWhenHit, GameObject explosionPrefab)
        {
            _damageWhenHit = damageWhenHit;
            _explosion = explosionPrefab;
            Vector3 startPos = transform.position;
            Vector3 endPos = targetPos;
            Vector3 controlPoint = (startPos + endPos) / 2;
            controlPoint.y += arcHeight;

            Vector3[] path = new Vector3[3];
            path[0] = startPos;
            path[1] = controlPoint;
            path[2] = endPos;

            transform.DOPath(path, duration, PathType.CatmullRom)
                     .SetEase(Ease.Linear)
                     .OnComplete(OnReachTarget);
        }

        void OnReachTarget()
        {
            var exp = Instantiate(_explosion, transform.position, Quaternion.identity);
            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].TakeDamage(_damageWhenHit);
                }
            }

            Destroy(exp, 1f);
            Invoke("Dispose", 2f);
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
