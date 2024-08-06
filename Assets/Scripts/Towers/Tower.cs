using Assets.Scripts.Map;
using Assets.Scripts.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Scripts.Towers
{
    public class Tower : MonoBehaviour
    {
        protected GameDataSO gameData; 
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

     
    }
}
