using Assets.Scripts.Data;
using Assets.Scripts.ScriptableObjects;
using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Manager
{
    public class FirebaseManager : MonoBehaviour
    {
        [Inject] private GameDataSO gameData;
        [Inject] private TowerPlacementManager towerPlacementManager;
        private DatabaseReference databaseReference;

        async void Start()
        {
            await InitializeFirebase();
        }

        private async Task InitializeFirebase()
        {

            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus != DependencyStatus.Available)
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
                return;
            }

            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseDatabase.GetInstance(app, "https://towerdefence-ed16a-default-rtdb.firebaseio.com/");
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Firebase initialized successfully");

            await LoadGameData();
        }

        public async Task SaveGameData()
        {
            if (databaseReference == null)
            {
                Debug.LogError("Database reference is not initialized.");
                return;
            }

            var data = new GameData
            {
                filledTurretTowerSlots = gameData.filledTurretTowerSlots,
                filledMortarTowerSlots = gameData.filledMortarTowerSlots,
                mineSetCount = gameData.mineSetCount,
                turretSetCount = gameData.turretSetCount,
                mortarSetCount = gameData.mortarSetCount,
                activeMineSlots = gameData.activeMineSlots,
                emptyTowerSlots = gameData.emptyTowerSlots,
                playerGold = gameData.playerGold,
                playerScore = gameData.playerScore,
                mainTowerHealth = gameData.mainTowerHealth,
                mainTowerMaxHP = gameData.mainTowerMaxHP,
                gameLevel = gameData.gameLevel,
                elapsedTime = gameData.elapsedTime,
                incrementTimer = gameData.incrementTimer,
                KillCount = gameData.KillCount
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            try
            {
                await databaseReference.Child("gameData").SetRawJsonValueAsync(json);
                Debug.Log("Game data saved successfully."); 
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to save game data: {ex.Message}");
            }
        }
         
        public async Task LoadGameData()
        {
            if (databaseReference == null)
            {
                Debug.LogError("Database reference is not initialized.");
                return;
            }

            try
            {
                var snapshot = await databaseReference.Child("gameData").GetValueAsync();

                Debug.Log($"Snapshot exists: {snapshot.Exists}, Value: {snapshot.GetRawJsonValue()}");

                if (!snapshot.Exists)
                {
                    Debug.Log("No data found in the database. Initializing with default values.");
                    await SaveGameData(); // Save default data if no data exists
                    return;
                }

                string json = snapshot.GetRawJsonValue();

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogWarning("Loaded JSON is null or empty.");
                    return;
                }

                var data = JsonConvert.DeserializeObject<GameData>(json);

                gameData.filledTurretTowerSlots = data.filledTurretTowerSlots;
                gameData.filledMortarTowerSlots = data.filledMortarTowerSlots;
                gameData.mineSetCount = data.mineSetCount;
                gameData.turretSetCount = data.turretSetCount;
                gameData.mortarSetCount = data.mortarSetCount;
                gameData.activeMineSlots = data.activeMineSlots;
                gameData.emptyTowerSlots = data.emptyTowerSlots;
                gameData.playerGold = data.playerGold;
                gameData.playerScore = data.playerScore;
                gameData.mainTowerHealth = data.mainTowerHealth;
                gameData.mainTowerMaxHP = data.mainTowerMaxHP;
                gameData.gameLevel = data.gameLevel;
                gameData.elapsedTime = data.elapsedTime;
                gameData.incrementTimer = data.incrementTimer; 

                Debug.Log("Game data loaded successfully.");
                towerPlacementManager.LoadOldScene();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load game data. Exception: {ex.GetType().Name}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }
    }

}