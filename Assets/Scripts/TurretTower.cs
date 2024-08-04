using Assets.Scripts.Pool;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class TurretTower : Tower
    {
        public int towerSlotIndex = -1;

        BulletMemoryPool _bulletPool; 
        [Inject]
        public void Construct(BulletMemoryPool bulletPool)
        {
            _bulletPool = bulletPool;
        }

        [ContextMenu("Shoot")]
        public void Shoot()
        {
            var bullet = _bulletPool.Spawn(); 
        }
         
    }
}
