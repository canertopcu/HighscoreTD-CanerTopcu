using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tower : MonoBehaviour
    {
        protected GameDataSO gameData;
        protected VfxDataSO vfxData;

        internal void SetGameData(GameDataSO gameData)
        {
            this.gameData = gameData;
        }

        internal void SetVfxData(VfxDataSO vfxData)
        {
            this.vfxData = vfxData;
        }
    }
}
