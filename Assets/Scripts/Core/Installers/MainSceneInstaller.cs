using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Signals;
using Assets.Scripts.Manager;
using Assets.Scripts.Pool;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        public VfxDataSO vfxDataItem;
        public GameDataSO gameDataItem;

        public GameObject bulletPrefab;
        public GameObject missilePrefab;
        public EnemySO normalEnemy;
        public EnemySO tinyEnemy;
        public EnemySO giantEnemy;
        public override void InstallBindings()
        { 
            Container.Bind<IGameManager>().To<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<LevelSignal>();
            Container.DeclareSignal<EnemyKillSignal>();
            Container.DeclareSignal<GameStateChangedSignal>();

            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle().NonLazy(); 

            Container.Bind<GameDataSO>().FromScriptableObject(gameDataItem).AsSingle();
            Container.Bind<VfxDataSO>().FromScriptableObject(vfxDataItem).AsSingle();

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

