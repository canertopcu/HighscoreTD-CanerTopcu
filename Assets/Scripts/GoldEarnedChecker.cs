
using Assets.Scripts.Core.Signals;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class GoldEarnedChecker : MonoBehaviour
    {

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<GoldEarnedSignal>(OnGoldEarned);
        }

        private void OnGoldEarned(GoldEarnedSignal signal)
        {
            Debug.LogError($"+{signal.Amount} Gold!");
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<GoldEarnedSignal>(OnGoldEarned);
        }

    }
}
