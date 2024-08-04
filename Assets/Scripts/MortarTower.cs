using Assets.Scripts.Pool;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class MortarTower : Tower
    {
        public int towerSlotIndex = -1;

        MissileMemoryPool _missilePool; 
        [Inject]
        public void Construct(MissileMemoryPool missilePool)
        {
            _missilePool = missilePool;
        }

        [ContextMenu("Shoot")]
        public void Shoot()
        {
            var missile = _missilePool.Spawn(); 
        }
         
    }
}
