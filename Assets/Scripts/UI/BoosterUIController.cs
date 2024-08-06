using Assets.Scripts.Core.Signals;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace Assets.Scripts.UI
{
    public class BoosterUIController : MonoBehaviour
    {
        public Button button;
        public Image boosterCoolDown;
        private GameDataSO _gameData;
        private SignalBus _signalBus;
        public int boosterActivationCount = 0;
        public int boosterActivationCountMax = 20;

        private bool isBoosterActive = false;
        private float boosterCoolDownTime = 5f;
        private float currentCoolDownTime;

        [Inject]
        private void Construct(SignalBus signalBus, GameDataSO gameDataSO)
        {
            _gameData = gameDataSO;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemyKillSignal>(OnEnemyKillSignal);
            boosterActivationCount = 0;
            boosterCoolDown.fillAmount = 0;
            button.onClick.AddListener(OnBoosterButtonClick);
            button.interactable = false;
        }

        private void OnBoosterButtonClick()
        {
            if (!isBoosterActive)
            {
                StartCoroutine(BoosterCoolDown());
                button.interactable = false;
            }
        }

        private void OnEnemyKillSignal(EnemyKillSignal enemyKillSignal)
        {
            if (boosterActivationCount < boosterActivationCountMax)
            {
                boosterCoolDown.color = Color.green;
                boosterActivationCount++;
                boosterCoolDown.fillAmount = (float)boosterActivationCount / boosterActivationCountMax;
                button.interactable = false;
            }
            else if (!isBoosterActive)
            {
                boosterCoolDown.color = Color.blue;
                button.interactable = true;
            }
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnBoosterButtonClick);
            _signalBus.Unsubscribe<EnemyKillSignal>(OnEnemyKillSignal);
        }

        private IEnumerator BoosterCoolDown()
        {
            isBoosterActive = true;
            _signalBus.Fire(new BoosterSignal(true));
            currentCoolDownTime = boosterCoolDownTime;

            while (currentCoolDownTime > 0)
            {
                boosterCoolDown.color = Color.red;
                boosterCoolDown.fillAmount = currentCoolDownTime / boosterCoolDownTime;
                currentCoolDownTime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            boosterCoolDown.color = Color.green;
            boosterCoolDown.fillAmount = 0;
            button.interactable = false;
            boosterActivationCount = 0;
            isBoosterActive = false;
            _signalBus.Fire(new BoosterSignal(false));
        }
    }
}