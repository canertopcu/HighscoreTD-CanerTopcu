using Assets.Scripts.Core.Signals;
using UnityEngine;
using Zenject;
namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnEnemyKilled(5);
            }

        }

        public void OnEnemyKilled(int goldAmount)
        {
            _signalBus.Fire(new GoldEarnedSignal(goldAmount));
        }
    }
}