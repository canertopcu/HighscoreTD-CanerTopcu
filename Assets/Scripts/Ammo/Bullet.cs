using Assets.Scripts.Enemy;
using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Ammo
{
    public class Bullet : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        IMemoryPool _pool;
        private GameObject _explosion;
        public float duration = 1f;
        EnemyController _enemy;
        private float _damageWhenHit;
        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }

        public void OnDespawned()
        {
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }


        [ContextMenu("Fire")]
        internal void Fire(EnemyController enemy, float damageWhenHit, GameObject explosionPrefab)// TowerSO towerData, GameDataSO gameData)
        {
            _damageWhenHit = damageWhenHit;
            _explosion = explosionPrefab;
            Vector3 startPos = transform.position;
            _enemy = enemy;
            transform.DOMove(enemy.transform.position, duration)
                    .SetEase(Ease.Linear)
                    .OnComplete(OnReachTarget);
        }

        void OnReachTarget()
        {
            _enemy.TakeDamage(_damageWhenHit);

            var exp = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Dispose();
        }



    }
}
