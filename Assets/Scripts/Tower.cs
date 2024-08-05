using Assets.Scripts.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tower : MonoBehaviour
    {
        protected GameDataSO gameData;
        protected VfxDataSO vfxData;
        protected TileElement selectedTileElement;

        public virtual void Setlment() { 
            
        }
        internal void SetGameData(GameDataSO gameData)
        {
            this.gameData = gameData;
            Setlment();
        }

        internal void SetSelectedTileElement(TileElement selectedTileElement)
        {
             this.selectedTileElement = selectedTileElement;
        }

        internal void SetVfxData(VfxDataSO vfxData)
        {
            this.vfxData = vfxData;
        }
    }
}
