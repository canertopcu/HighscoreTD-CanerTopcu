using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Signals;
using Assets.Scripts.ScriptableObjects;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        private SignalBus _signalBus;
        private GameDataSO _gameDataSO;
        private UIManager _uiManager;
        private FirebaseManager _firebaseManager;
        public bool isGameStarted = false;

        [Inject]
        private void Construct(SignalBus signalBus, GameDataSO gameDataSO, UIManager uiManager, FirebaseManager firebaseManager)
        {
            _firebaseManager = firebaseManager;
            _signalBus = signalBus;
            _gameDataSO = gameDataSO;
            _uiManager = uiManager;
        }

        async void Start()
        {

            if (_gameDataSO == null)
            {
                Debug.LogError("GameDataSO is not injected!");
                return;
            }
            await Task.Delay(1000);
            _firebaseManager.LoadGameData();

            if (_uiManager == null)
            {
                Debug.LogError("UIManager is not injected!");
                return;
            }

            if (_gameDataSO.mainTowerHealth == 0)
            {
                _gameDataSO.ResetElements();
            } 
        }

        public void StartGame()
        {
            if (_signalBus == null)
            {
                Debug.LogError("SignalBus is not injected!");
                return;
            }

            isGameStarted = true;
            _signalBus.Fire(new GameStateChangedSignal(isGameStarted));
        }

        public void EndGame()
        {
            if (_signalBus == null)
            {
                Debug.LogError("SignalBus is not injected!");
                return;
            }

            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (highScore < _gameDataSO.playerScore)
            {
                PlayerPrefs.SetInt("HighScore", _gameDataSO.playerScore);
                highScore = _gameDataSO.playerScore;
            }

            _uiManager.ShowGameOverPanel(_gameDataSO.playerScore, highScore);
            isGameStarted = false;
            _signalBus.Fire(new GameStateChangedSignal(isGameStarted));
        }

        public bool IsGameStarted()
        {
            return isGameStarted;
        }

        private void Update()
        {
            if (!isGameStarted)
                return;

            _gameDataSO.elapsedTime += Time.deltaTime;
            var calculatedLevel = (int)(_gameDataSO.elapsedTime / _gameDataSO.incrementTimer);
            if (calculatedLevel != _gameDataSO.gameLevel)
            {
                _gameDataSO.gameLevel = calculatedLevel;
                _signalBus.Fire(new LevelSignal(_gameDataSO.gameLevel));
            }
        }
        void OnApplicationQuit()
        {
            SaveGameData();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveGameData();
            }
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveGameData();
            }
        }

        private void SaveGameData()
        {
            _firebaseManager.SaveGameData();
        }
    }
}
