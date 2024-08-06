using Assets.Scripts.Ammo;
using System.Reflection;
using Zenject; 

namespace Assets.Scripts.Pool
{
    public class MissileMemoryPool : MonoMemoryPool<Missile>
    {
        protected override void Reinitialize(Missile item)
        {
            item.OnSpawned(this);
        }
    }
}
