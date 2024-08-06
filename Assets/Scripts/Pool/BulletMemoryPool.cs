using Assets.Scripts.Ammo;
using Zenject; 

namespace Assets.Scripts.Pool
{
    public class BulletMemoryPool : MonoMemoryPool<Bullet>
    {
        protected override void Reinitialize(Bullet item)
        {
            item.OnSpawned(this);
        }
    }
}
