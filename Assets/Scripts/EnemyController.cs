using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    IMemoryPool _pool;


    WaypointManager waypointManager;
    GameDataSO gameData;

    bool isStarted = false;
    Transform selectedPoint;
    public int health = 10;
    public int damage = 10;

    public bool isDanced = false;
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
        // Initialization logic here
    }

    public void OnDespawned()
    {
        // Cleanup logic here
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    private void Die()
    {
        // Logic for enemy death
        Dispose();
    }

    internal void StartMovement()
    {
        isStarted = true;
        selectedPoint = waypointManager.GetFirstPoint();
        transform.position = selectedPoint.position;
        transform.LookAt(waypointManager.GetNextPoint(selectedPoint), Vector3.up);
    }

    private void Update()
    {
        if (isStarted && gameData.mainTowerHealth>0)
        {
            var nextPoint = waypointManager.GetNextPoint(selectedPoint);
            transform.position = Vector3.MoveTowards(transform.position, nextPoint.position, Time.deltaTime);
            transform.LookAt(nextPoint, Vector3.up);

            if (Vector3.Distance(transform.position, nextPoint.position) < 0.1f)
            {
                selectedPoint = nextPoint;
                if (waypointManager.GetNextPoint(selectedPoint) == null)
                {
                    isStarted = false;
                    Debug.Log("Enemy reached the end");
                    StartCoroutine(JumpAndDispose());
                }
            }
        }

        if (isStarted && gameData.mainTowerHealth == 0 && !isDanced) { 
            isDanced= true;
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            animator.SetTrigger("Dancing");
            animator.SetFloat("DancingSpeed", UnityEngine.Random.Range(0.3f,2f));
        }
    }

    IEnumerator JumpAndDispose()
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
        animator.SetTrigger("Jump");
        gameData.HitDamageMainTower(damage);
        yield return new WaitForSeconds(1.5f);
        Dispose();
    }

    internal void Setup(WaypointManager waypointManager, GameDataSO gameData)
    {
        this.gameData = gameData;
        this.waypointManager = waypointManager;
    }
}
