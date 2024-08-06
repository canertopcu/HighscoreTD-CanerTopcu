using Assets.Scripts.Core.Signals;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BoosterUIController : MonoBehaviour
{
    public Button button;
    public Image boosterCoolDown;
    GameDataSO _gameData;
    SignalBus _signalBus;
    public int boosterActivationCount = 0;
    public int boosterActivationCountMax = 20;

    bool isBoosterActive = false;
    public float boosterCoolDownTime = 5f;

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
        StartCoroutine(BoosterCoolDown());
        button.interactable = false;
    }

    private void OnEnemyKillSignal(EnemyKillSignal enemyKillSignal)
    {
        if (boosterActivationCount < boosterActivationCountMax)
        {
            boosterCoolDown.color = Color.green;
            boosterActivationCount++;
            boosterCoolDown.fillAmount =  (float)boosterActivationCount / boosterActivationCountMax;
            button.interactable = false;
        }
        else
        {
            boosterCoolDown.color = Color.blue;
            boosterActivationCount = 0;
            button.interactable = true;

        }

    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnBoosterButtonClick);
        _signalBus.Unsubscribe<EnemyKillSignal>(OnEnemyKillSignal);
    }

    IEnumerator BoosterCoolDown()
    {
        while (0 < boosterCoolDownTime)
        {
            boosterCoolDown.color=Color.red;
            boosterCoolDown.fillAmount = boosterCoolDownTime / 5f;
            boosterCoolDownTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        boosterCoolDown.color = Color.green;
        boosterCoolDown.fillAmount = 0;
        button.interactable = false;
    }
}
