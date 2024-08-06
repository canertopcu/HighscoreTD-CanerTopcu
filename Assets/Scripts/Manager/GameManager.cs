using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Signals;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;
using Zenject.ReflectionBaking.Mono.Cecil;
namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        private SignalBus _signalBus;

        [Inject] GameDataSO gameDataSO;

        [Inject] UIManager uiManager;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
         
        private void Start()
        {
            if (gameDataSO.mainTowerHealth == 0)
                gameDataSO.ResetElements();
        }

        public bool isGameStarted = false;
        public void StartGame()
        {
            isGameStarted = true;
            _signalBus.Fire(new GameStateChangedSignal(isGameStarted));
        }

        public void EndGame()
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (highScore < gameDataSO.playerScore)
            {
                PlayerPrefs.SetInt("HighScore", gameDataSO.playerScore);
                highScore = gameDataSO.playerScore;
            }
            uiManager.ShowGameOverPanel(gameDataSO.playerScore, highScore);
            isGameStarted = false;
            _signalBus.Fire(new GameStateChangedSignal(isGameStarted));
        }

        public bool IsGameStarted()
        {
            return isGameStarted;
        }

        private void Update()
        {
            if (isGameStarted)
            {
                gameDataSO.elapsedTime += Time.deltaTime;
                var calculatedLevel = (int)(gameDataSO.elapsedTime / gameDataSO.incrementTimer);
                if (calculatedLevel != gameDataSO.gameLevel)
                {
                    gameDataSO.gameLevel = calculatedLevel;
                    _signalBus.Fire(new LevelSignal(gameDataSO.gameLevel));
                } 
            }
        }
    }
}
