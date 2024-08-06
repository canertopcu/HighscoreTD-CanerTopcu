using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace Assets.Scripts.UI
{
    public class MainTowerHPBarUIController : MonoBehaviour
    {
        [Inject]
        GameDataSO gameData;

        public Image hpBarImage;

        private void Update()
        {
            SetHPBarValue((float)gameData.mainTowerHealth / gameData.mainTowerMaxHP);
        }

        public void SetHPBarValue(float value)
        {
            hpBarImage.fillAmount = value;
        }
    }
}