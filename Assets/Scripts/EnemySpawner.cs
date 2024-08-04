using Assets.Scripts.Pool;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    EnemyMemoryPool _enemyPool;

    [Inject] GameDataSO gameData;

    [Inject]
    public void Construct(EnemyMemoryPool enemyPool)
    {
        _enemyPool = enemyPool;
    }

    [Inject] WaypointManager waypointManager;
    public bool isStarted = false;

    public float spawnRate = 1f;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitUntil(() => isStarted);
        while (isStarted)
        {

            var enemy = _enemyPool.Spawn();
            //var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyController>();
            enemy.Setup(waypointManager, gameData);
            enemy.StartMovement();
            yield return new WaitForSeconds(spawnRate);
        }
    }

}
