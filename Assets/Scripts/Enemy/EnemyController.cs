using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Signals;
using Assets.Scripts.Manager;
using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        IMemoryPool _pool;
        
        private GameDataSO _gameData; 
        private EnemySO _enemyData;

        private WaypointManager _waypointManager;
        private bool isStarted = false;
        private bool isDisposed = false;
        private Transform selectedPoint;

        private bool _isDanced = false;
        private int _health = 100;

        private IGameManager _gameManager;
        private SignalBus _signalBus;
        private EnemyManager _enemyManager;

        [Inject]
        private void Construct(IGameManager gameManager, WaypointManager waypointManager, GameDataSO gameData, SignalBus signalBus, EnemyManager enemyManager)
        {
            _gameManager = gameManager;
            _gameData = gameData;
            _waypointManager = waypointManager;
            _signalBus = signalBus;
            _enemyManager = enemyManager;
        }

        private Animator _animator;
        private Animator animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = GetComponent<Animator>();
                }
                return _animator;
            }
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _health = (int)(_enemyData.Hp * Mathf.Pow(_enemyData.hpMultiplier, _gameData.gameLevel));
            isStarted = false;
            isDisposed = false;
            _isDanced = false;
        }

        public void OnDespawned()
        {
            // Cleanup logic here
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                _enemyManager.RemoveEnemy(this);
                _pool.Despawn(this);
            }
        }

        private void Die()
        {
            if (!isDisposed)
            {
                _gameData.AddKillCount();
                _signalBus.Fire(new EnemyKillSignal());
                Dispose();
            }
        }

        internal void StartMovement()
        {
            isStarted = true;
            selectedPoint = _waypointManager.GetFirstPoint();
            transform.position = selectedPoint.position;
            transform.LookAt(_waypointManager.GetNextPoint(selectedPoint), Vector3.up);
        }

        private void Update()
        {
            if (isStarted && _gameData.mainTowerHealth > 0)
            {
                var nextPoint = _waypointManager.GetNextPoint(selectedPoint);
                if (nextPoint != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextPoint.position, Time.deltaTime);
                    transform.LookAt(nextPoint, Vector3.up);

                    if (Vector3.Distance(transform.position, nextPoint.position) < 0.1f)
                    {
                        selectedPoint = nextPoint;
                        if (_waypointManager.GetNextPoint(selectedPoint) == null)
                        {
                            isStarted = false;
                            Debug.Log("Enemy reached the end");
                            StartCoroutine(JumpAndDispose());
                        }
                    }
                }
            }

            if (isStarted && _gameData.mainTowerHealth == 0 && !_isDanced)
            {
                _isDanced = true;
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
                animator.SetTrigger("Dancing");
                animator.SetFloat("DancingSpeed", UnityEngine.Random.Range(0.3f, 2f));
            }
        }

        IEnumerator JumpAndDispose()
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            animator.SetTrigger("Jump");
            _gameData.HitDamageMainTower(_enemyData.attackDamage);

            if (_gameData.mainTowerHealth == 0)
            {
                _gameManager.EndGame();
            }
            yield return new WaitForSeconds(1.5f);
            Dispose();
        }

        internal void TakeDamage(float attackDamage)
        {
            _health -= (int)attackDamage;
            if (_health <= 0)
            {
                _health = 0;
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);

                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    animator.SetTrigger("Hit1");
                }
                else
                {
                    animator.SetTrigger("Hit2");
                }

                _gameData.AddMoney(_enemyData.goldReward, _enemyData.goldRewardMultiplier);
                _gameData.AddScore(_enemyData.scoreReward, _enemyData.scoreRewardMultiplier);

                isStarted = false;
                if (transform.gameObject.activeSelf)
                {
                    Invoke("Die", 2f);
                }
            }
        }
    }
}