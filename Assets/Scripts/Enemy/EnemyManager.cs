using Assets.Scripts.Core.Signals;
using Assets.Scripts.Pool;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyController> enemies = new List<EnemyController>();
    NormalEnemyMemoryPool _normalEnemyPool;
    TinyEnemyMemoryPool _tinyEnemyPool;
    GiantEnemyMemoryPool _giantEnemyPool;

    [Inject] GameDataSO gameData;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(NormalEnemyMemoryPool enemyPool, TinyEnemyMemoryPool tinyEnemyPool, GiantEnemyMemoryPool giantEnemyPool, SignalBus signalBus)
    {
        _normalEnemyPool = enemyPool;
        _tinyEnemyPool = tinyEnemyPool;
        _giantEnemyPool = giantEnemyPool;
        _signalBus = signalBus;
    }

    [Inject] WaypointManager waypointManager;
    public bool isStarted = false;

    public float spawnRate = 1f;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged); 
        _signalBus.Subscribe<LevelSignal>(OnLevelChanged); 
    }

    private void OnLevelChanged(LevelSignal levelSignal)
    { 
        spawnRate = Mathf.Clamp(5f - (levelSignal.Level * 0.02f),1f,5f);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        _signalBus.Unsubscribe<LevelSignal>(OnLevelChanged);
    }

    private void OnGameStateChanged(GameStateChangedSignal stateKeeper)
    {
        isStarted = stateKeeper.State;
        spawnRate = Mathf.Clamp(5f - (gameData.gameLevel * 0.02f), 1f, 5f); ;
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitUntil(() => isStarted);
        while (isStarted)
        {

            EnemyController enemy;
            switch (Random.Range(0, 3))
            {
                case 0:
                    enemy = _normalEnemyPool.Spawn();

                    break;
                case 1:
                    enemy = _tinyEnemyPool.Spawn();
                    break;
                case 2:
                    enemy = _giantEnemyPool.Spawn();
                    break;
                default:
                    enemy = _normalEnemyPool.Spawn();
                    break;
            }
            enemies.Add(enemy);
            enemy.StartMovement();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }

    public EnemyController GetClosestEnemy(Vector3 position, float range)
    {
        EnemyController closestEnemy = null;
        float minDistance = Mathf.Infinity;
        
        foreach (var enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < range && distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
