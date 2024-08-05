using Assets.Scripts.Core.Signals;
using Assets.Scripts.Pool;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
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
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedSignal stateKeeper)
    {
        isStarted = stateKeeper.State;
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

            //var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyController>();
            enemy.Setup(waypointManager, gameData);
            enemy.StartMovement();
            yield return new WaitForSeconds(spawnRate);
        }
    }

}
