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
        public GameObject enemyPrefab;
        public override void InstallBindings()
        {
            //Container.Bind<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();  
            Container.Bind<IGameManager>().To<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<GoldEarnedSignal>();

            Container.Bind<PlayerController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<GoldEarnedChecker>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

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

            Container.BindMemoryPool<EnemyController, EnemyMemoryPool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(enemyPrefab) 
                .UnderTransformGroup("Enemies");

             



        }
    }
}

