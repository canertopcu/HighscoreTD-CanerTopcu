using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Signals;
using Assets.Scripts.Manager;
using Assets.Scripts.ScriptableObjects;
using Zenject;

namespace Assets.Scripts.Core.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        public GameDataSO gameDataPrefab;
        public override void InstallBindings()
        {
            //Container.Bind<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();  
            Container.Bind<IGameManager>().To<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<GoldEarnedSignal>();

            Container.Bind<PlayerController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<GoldEarnedChecker>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<GameDataSO>().FromScriptableObject(gameDataPrefab).AsSingle();
            Container.Bind<TowerPlacementManager>().FromComponentInHierarchy().AsSingle();


        }
    }
}