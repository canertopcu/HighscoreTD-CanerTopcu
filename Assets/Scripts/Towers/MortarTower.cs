using Assets.Scripts.Core.Signals;
using Assets.Scripts.Enemy;
using Assets.Scripts.Pool;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Towers
{
    public class MortarTower : Tower
    {
        public int towerSlotIndex = -1;
        public Transform muzzle;
        public EnemyController target;
        public TowerSO towerData;
        MissileMemoryPool _missilePool;
        EnemyManager _enemyManager;
        SignalBus _signalBus;
        float timer = 0;
        bool state = true;
        bool isBoosterActive = false;
        private float _damageWhenHit;
        [Inject]
        public void Construct(MissileMemoryPool missilePool, EnemyManager enemyManager, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _enemyManager = enemyManager;
            _missilePool = missilePool;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
            _signalBus.Subscribe<BoosterSignal>(OnBoosterSignal);
        }

        private void OnBoosterSignal(BoosterSignal boosterSignal)
        {
            isBoosterActive = boosterSignal.IsBoosterActive;
            timer = towerData.coolDown * (isBoosterActive ? 0.5f : 1);
        }

        private void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
        {
            state = gameStateChangedSignal.State;
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
            _signalBus.Unsubscribe<BoosterSignal>(OnBoosterSignal);
        }

        [ContextMenu("Shoot")]
        public void Shoot()
        {
            var missile = _missilePool.Spawn();
            missile.transform.position = muzzle.position;
            missile.transform.rotation = muzzle.rotation;
            _damageWhenHit = towerData.attackDamage * Mathf.Pow(towerData.attackMultiplier, gameData.gameLevel);
            missile.MoveProjectile(target.transform.position, _damageWhenHit, towerData.explosionPrefab);
            timer = towerData.coolDown * (isBoosterActive ? 0.5f : 1);
        }


        public void FixedUpdate()
        {
            if (towerSlotIndex != -1)
            {
                target = _enemyManager.GetClosestEnemy(transform.position, towerData.attackRange);
            }
        }
        private void Update()
        {
            if (target != null && state)
            {
                Vector3 pos1 = target.transform.position;
                Vector3 pos2 = transform.position;
                Vector3 dir = pos1 - pos2;
                dir.y = 0;
                transform.eulerAngles = new Vector3(-90, 180 + Vector3.Angle(Vector3.right, dir), 90);
                if (timer <= 0)
                {
                    Shoot();
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
        }
    }
}
