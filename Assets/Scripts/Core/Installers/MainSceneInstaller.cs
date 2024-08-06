using Assets.Scripts.Ammo;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Signals;
using Assets.Scripts.Enemy;
using Assets.Scripts.Manager;
using Assets.Scripts.Map;
using Assets.Scripts.Pool;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Installers
{
    public class MainSceneInstaller : MonoInstaller
    { 
        [SerializeField] private GameDataSO gameDataItem; 
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private EnemySO normalEnemy;
        [SerializeField] private EnemySO tinyEnemy;
        [SerializeField] private EnemySO giantEnemy;
        public override void InstallBindings()
        { 
            Container.Bind<IGameManager>().To<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<FirebaseManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<LevelSignal>();
            Container.DeclareSignal<EnemyKillSignal>();
            Container.DeclareSignal<GameStateChangedSignal>();
            Container.DeclareSignal<BoosterSignal>();

            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle().NonLazy(); 

            Container.Bind<GameDataSO>().FromScriptableObject(gameDataItem).AsSingle();

            Container.Bind<EnemyManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TowerPlacementManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<WaypointManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<MapController>().FromComponentInHierarchy().AsSingle();

            //Pool
            Container.BindMemoryPool<Bullet, BulletMemoryPool>()
               .WithInitialSize(20)
               .FromComponentInNewPrefab(bulletPrefab)
               .UnderTransformGroup("Bullets");

            Container.BindMemoryPool<Missile, MissileMemoryPool>()
              .WithInitialSize(20)
              .FromComponentInNewPrefab(missilePrefab)
              .UnderTransformGroup("Missles");

            Container.BindMemoryPool<EnemyController, NormalEnemyMemoryPool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(normalEnemy.enemyPrefab)
                .UnderTransformGroup("Enemies1");
            Container.BindMemoryPool<EnemyController, TinyEnemyMemoryPool>()
              .WithInitialSize(10)
              .FromComponentInNewPrefab(tinyEnemy.enemyPrefab)
              .UnderTransformGroup("Enemies2");
            Container.BindMemoryPool<EnemyController, GiantEnemyMemoryPool>()
              .WithInitialSize(10)
              .FromComponentInNewPrefab(giantEnemy.enemyPrefab)
              .UnderTransformGroup("Enemies3");





        }
    }
}

