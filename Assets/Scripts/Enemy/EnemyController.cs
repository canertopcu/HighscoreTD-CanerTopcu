using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Signals;
using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    IMemoryPool _pool;
    public EnemySO enemyData;
    WaypointManager _waypointManager;
    GameDataSO _gameData;

    bool isStarted = false;
    Transform selectedPoint;

    public bool isDanced = false;
    public int health = 100;

    private IGameManager _gameManager;
    private SignalBus _signalBus;
    private EnemyManager _enemyManager;
 
    [Inject]
    private void Construct(IGameManager gameManager, WaypointManager waypointManager, GameDataSO gameData,SignalBus signalBus,EnemyManager enemyManager)
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
        health = (int)(enemyData.Hp * Mathf.Pow(enemyData.hpMultiplier, _gameData.gameLevel));

        // Initialization logic here
    }

    public void OnDespawned()
    {
        // Cleanup logic here
    }

    public void Dispose()
    {
        _enemyManager.RemoveEnemy(this);
        _pool.Despawn(this);
    }

    private void Die()
    {
        // Logic for enemy death
        _gameData.AddKillCount();
        _signalBus.Fire(new EnemyKillSignal());
        Dispose();
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

        if (isStarted && _gameData.mainTowerHealth == 0 && !isDanced)
        {
            isDanced = true;
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
        _gameData.HitDamageMainTower(enemyData.attackDamage);

        if (_gameData.mainTowerHealth == 0)
        {
            _gameManager.EndGame();
        }
        yield return new WaitForSeconds(1.5f);
        Dispose();
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    internal void TakeDamage(float attackDamage)
    {
        health -= (int)attackDamage;
        if (health <= 0)
        {
            health = 0;
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

            _gameData.AddMoney(enemyData.goldReward, enemyData.goldRewardMultiplier);
            _gameData.AddScore(enemyData.scoreReward, enemyData.scoreRewardMultiplier);

            isStarted = false;
            StartCoroutine(KillSlowly());
        }
    }

    IEnumerator KillSlowly()
    {
        yield return new WaitForSeconds(2f);
        Die();
    }
}
